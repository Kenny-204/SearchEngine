using DocumentFormat.OpenXml.Packaging;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  /// <summary>
  /// Parser for reading content from DOCX files
  /// </summary>
  public class DocxParser : IParser
  {
    /// <summary>
    /// Reads the content of a DOCX file
    /// </summary>
    /// <param name="filePath">The path to the DOCX file</param>
    /// <returns>The plain text content of the file</returns>
    public string ReadContent(string filePath)
    {
      using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
      {
        var body = wordDoc?.MainDocumentPart?.Document?.Body;
        return body!.InnerText;
      }
    }
  }
}
