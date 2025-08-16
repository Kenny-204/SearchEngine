using System.Xml.Linq;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class XMLParser : IParser
  {
    public string ReadContent(string filePath)
    {
      XDocument xmlDoc = XDocument.Load("xmlfile.xml");
      string text = xmlDoc.Root!.Value;
      return text;
    }
  }
}
