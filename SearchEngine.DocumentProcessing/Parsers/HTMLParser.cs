using HtmlAgilityPack;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class HTMLParser : IParser
  {
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
