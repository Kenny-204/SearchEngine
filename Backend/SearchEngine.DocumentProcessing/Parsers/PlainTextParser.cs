using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  /// <summary>
  /// Parser for reading content from TXT files
  /// </summary>
  public class PlainTextParser : IParser
  {
    /// <summary>
    /// Reads the content of a TXT file
    /// </summary>
    /// <param name="filePath">The path to the TXT file</param>
    /// <returns>The plain text content of the file</returns>
    public string ReadContent(string filePath)
    {
      return File.ReadAllText(filePath);
    }
  }
}
