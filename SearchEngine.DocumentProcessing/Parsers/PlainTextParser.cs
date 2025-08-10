using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class PlainTextParser : IParser
  {
    public string ReadContent(string filePath)
    {
      return File.ReadAllText("text.txt");
    }
  }
}
