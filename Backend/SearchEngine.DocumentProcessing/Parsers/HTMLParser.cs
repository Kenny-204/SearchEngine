using HtmlAgilityPack;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  /// <summary>
  /// Parser for reading content from HTML files
  /// </summary>
  public class HTMLParser : IParser
  {
    /// <summary>
    /// Reads the content of a HTML file
    /// </summary>
    /// <param name="filePath">The path to the HTML file</param>
    /// <returns>The plain text content of the file</returns>
    public string ReadContent(string filePath)
    {
      var htmlDoc = new HtmlDocument();
      htmlDoc.Load(filePath);
      var node = htmlDoc.DocumentNode.SelectSingleNode("//body");
      string text = node.InnerText;
      return text;
    }
  }
}
