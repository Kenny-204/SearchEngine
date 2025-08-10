using SearchEngine.DocumentProcessing.Interfaces;
using SearchEngine.DocumentProcessing.Parsers;

namespace SearchEngine.DocumentProcessing.Services
{
  public class ParserFactory
  {
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
        default:
          throw new Exception($"File type {extension} not supported");
      }
    }
  }
}
