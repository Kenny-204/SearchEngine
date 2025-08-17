using SearchEngine.DocumentProcessing.Interfaces;
using SearchEngine.DocumentProcessing.Parsers;

namespace SearchEngine.DocumentProcessing.Services
{
  /// <summary>
  /// /Factory for creatinf appropriate parser instances based on file extension.
  /// </summary>
  public class ParserFactory
  {
    /// <summary>
    /// returns an <see cref="IParser" instance based on file extension/>
    /// </summary>
    /// <param name="filePath"><The path to the document file./param>
    /// <returns>An <see cref="IParser"/> capable of parsing the given file</returns>
    /// <exception cref="Exception">Thrown if the file path is not supported</exception>
    public static IParser GetParser(string filePath)
    {
      string extension = Path.GetExtension(filePath).ToLower();
      switch (extension)
      {
        case ".txt":
          return new PlainTextParser();
        case ".html":
          return new HTMLParser();
        case ".xml":
          return new XMLParser();
        case ".pdf":
          return new PDFParser();
        case ".docx":
          return new DocxParser();
        case ".pptx":
          return new PptxParser();
        case ".xlsx":
          return new Xlsxparser();
        default:
          throw new Exception($"File type {extension} not supported");
      }
    }
  }
}
