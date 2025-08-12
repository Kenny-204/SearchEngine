using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
// using DocumentFormat.OpenXml.Presentation;
// using DocumentFormat.OpenXml.Spreadsheet;
// using DocumentFormat.OpenXml.Wordprocessing;
using ExcelDataReader;
using HtmlAgilityPack;
using UglyToad.PdfPig;

namespace SearchEngine.API.Core;

public class DocumentMetadata
{
  public string? Author { get; set; }
  public string Title { get; set; } = string.Empty;
  public required string FileName { get; set; }
  public required string Extension { get; set; }
  public long FileSizeBytes { get; set; }
  public int WordCount { get; set; }
  public int PageCount { get; set; } // PDF, DOCX, PPTX only
  public DateTime? CreatedDate { get; set; }
}

public class DocumentProcessor
{
  public (DocumentMetadata, Dictionary<string, int>, List<string>) ParseDocument(
    Stream fileStream,
    string fileName,
    string ext
  )
  {
    var terms = ProcessFile(fileStream, ext);
    var keywords = GetKeywords(terms);

    DocumentMetadata meta = ExtractMetadata(fileStream, fileName, ext);
    meta.WordCount = terms.Values.Sum();
    return (meta, terms, keywords);
  }

  private List<string> GetKeywords(Dictionary<string, int> dict)
  {
    double mean = dict.Values.Average();
    double stdDev = Math.Sqrt(dict.Values.Select(f => Math.Pow(f - mean, 2)).Average());

    var significantTerms = dict.Where(kv => kv.Value > mean + 1.5 * stdDev);
    return significantTerms.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();
  }

  private DocumentMetadata ExtractMetadata(Stream fileStream, string fileName, string ext)
  {
    var metadata = new DocumentMetadata
    {
      FileName = fileName,
      Extension = ext,
      FileSizeBytes = fileStream.Length,
    };

    fileStream.Position = 0;
    switch (metadata.Extension)
    {
      case ".pdf":
        using (var pdf = PdfDocument.Open(fileStream))
        {
          metadata.PageCount = pdf.NumberOfPages;

          // Extract Title and Author from PDF metadata dictionary (if available)
          var info = pdf.Information;

          if (!string.IsNullOrWhiteSpace(info.Title))
            metadata.Title = info.Title;

          if (!string.IsNullOrWhiteSpace(info.Author))
            metadata.Author = info.Author;
        }
        break;

      case ".docx":
        using (var doc = WordprocessingDocument.Open(fileStream, false))
        {
          metadata.PageCount =
            doc.ExtendedFilePropertiesPart?.Properties?.Pages?.Text != null
              ? int.Parse(doc.ExtendedFilePropertiesPart.Properties.Pages.Text)
              : 0;

          if (doc.PackageProperties.Created.HasValue)
            metadata.CreatedDate = doc.PackageProperties.Created.Value;

          // Title and Author
          if (!string.IsNullOrEmpty(doc.PackageProperties.Title))
            metadata.Title = doc.PackageProperties.Title;

          if (!string.IsNullOrEmpty(doc.PackageProperties.Creator))
            metadata.Author = doc.PackageProperties.Creator;
        }
        break;

      case ".pptx":
        using (var ppt = PresentationDocument.Open(fileStream, false))
        {
          metadata.PageCount = ppt.PresentationPart?.SlideParts.Count() ?? 0;

          if (ppt.PackageProperties.Created.HasValue)
            metadata.CreatedDate = ppt.PackageProperties.Created.Value;

          // Title and Author
          if (!string.IsNullOrEmpty(ppt.PackageProperties.Title))
            metadata.Title = ppt.PackageProperties.Title;

          if (!string.IsNullOrEmpty(ppt.PackageProperties.Creator))
            metadata.Author = ppt.PackageProperties.Creator;
        }
        break;

      case ".xlsx":
        using (var xls = SpreadsheetDocument.Open(fileStream, false))
        {
          metadata.PageCount = xls.WorkbookPart?.Workbook.Sheets?.Count() ?? 0;

          if (xls.PackageProperties.Created.HasValue)
            metadata.CreatedDate = xls.PackageProperties.Created.Value;

          // Title and Author
          if (!string.IsNullOrEmpty(xls.PackageProperties.Title))
            metadata.Title = xls.PackageProperties.Title;

          if (!string.IsNullOrEmpty(xls.PackageProperties.Creator))
            metadata.Author = xls.PackageProperties.Creator;
        }
        break;

      case ".html":
        {
          // Parse HTML and extract <title> tag
          using (
            var reader = new StreamReader(fileStream, Encoding.UTF8, true, 1024, leaveOpen: true)
          )
          {
            var html = reader.ReadToEnd();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var titleNode = doc.DocumentNode.SelectSingleNode("//title");
            if (titleNode != null)
              metadata.Title = titleNode.InnerText.Trim();
          }
        }
        break;

      case ".txt":
        {
          // Read first non-empty line as title
          using (
            var reader = new StreamReader(fileStream, Encoding.UTF8, true, 1024, leaveOpen: true)
          )
          {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
              if (!string.IsNullOrWhiteSpace(line))
              {
                metadata.Title = line.Trim();
                break;
              }
            }
          }
        }
        break;

      case ".xml":
        {
          try
          {
            // Load XML, use root element name as title
            XDocument xdoc = XDocument.Load(fileStream);
            metadata.Title = xdoc.Root?.Name.LocalName ?? "XML Document";
          }
          catch
          {
            metadata.Title = "XML Document";
          }
        }
        break;
    }

    return metadata;
  }

  private Dictionary<string, int> ProcessFile(Stream fileStream, string ext)
  {
    fileStream.Position = 0;
    string text = ext switch
    {
      ".pdf" => ExtractPdf(fileStream),
      ".docx" => ExtractDocx(fileStream),
      ".doc" => ExtractDocx(fileStream), // For old DOC use NPOI if needed
      ".pptx" => ExtractPptx(fileStream),
      ".ppt" => ExtractPptx(fileStream),
      ".xlsx" => ExtractXlsx(fileStream),
      ".xls" => ExtractXlsx(fileStream),
      ".txt" => ReadTxt(fileStream),
      ".html" or ".htm" => ExtractHtml(fileStream),
      ".xml" => ExtractXml(fileStream),
      _ => throw new NotSupportedException("Unsupported file type: " + ext),
    };

    var tokens = TokenizeAndNormalize(text);
    return tokens.GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count());
  }

  private string ExtractPdf(Stream stream)
  {
    using var copy = new MemoryStream();
    stream.CopyTo(copy);
    copy.Position = 0;
    using var doc = PdfDocument.Open(copy);
    return string.Join("\n", doc.GetPages().Select(p => p.Text));
  }

  private string ExtractDocx(Stream stream)
  {
    using var copy = new MemoryStream();
    stream.CopyTo(copy);
    copy.Position = 0;
    using var doc = WordprocessingDocument.Open(copy, false);
    var _ = doc
      .MainDocumentPart?.Document.Body?.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
      .Select(t => t.Text);
    return string.Join(" ", _ ?? []);
  }

  private string ExtractPptx(Stream stream)
  {
    using var copy = new MemoryStream();
    stream.CopyTo(copy);
    copy.Position = 0;
    using var ppt = PresentationDocument.Open(copy, false);
    var _ = ppt
      .PresentationPart?.SlideParts.SelectMany(s =>
        s.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>()
      )
      .Select(t => t.Text);
    return string.Join(" ", _ ?? []);
  }

  private string ExtractXlsx(Stream stream)
  {
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    using var reader = ExcelReaderFactory.CreateReader(
      stream,
      new ExcelReaderConfiguration { LeaveOpen = true }
    );

    var textParts = new List<string>();
    do
    {
      while (reader.Read())
      {
        for (int col = 0; col < reader.FieldCount; col++)
        {
          var val = reader.GetValue(col)?.ToString();
          if (!string.IsNullOrWhiteSpace(val))
            textParts.Add(val);
        }
      }
    } while (reader.NextResult());

    return string.Join(" ", textParts);
  }

  private string ReadTxt(Stream stream)
  {
    using var reader = new StreamReader(stream, leaveOpen: true);
    return reader.ReadToEnd();
  }

  private string ExtractHtml(Stream stream)
  {
    var doc = new HtmlDocument();
    doc.Load(stream);
    return doc.DocumentNode.InnerText;
  }

  private string ExtractXml(Stream stream)
  {
    var xmlDoc = new XmlDocument();
    xmlDoc.Load(stream);
    return xmlDoc.InnerText;
  }

  private List<string> TokenizeAndNormalize(string text)
  {
    return Regex
      .Split(text.ToLowerInvariant(), @"\W+")
      .Where(token => token.Length > 0 && !Globals.StopWords.Contains(token))
      .ToList();
  }
}
