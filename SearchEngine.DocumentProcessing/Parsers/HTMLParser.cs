using HtmlAgilityPack;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class HTMLParser : IParser
  {
    /// <summary>
    /// Parses an HTML file
    /// </summary>
    /// <param name="filePath">the path to the file</param>
    /// <returns>The text of the file</returns>
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
