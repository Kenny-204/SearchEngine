using System.Xml.Linq;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  /// <summary>
  /// Parser for reading content from XML files
  /// </summary>
  public class XMLParser : IParser
  {
    /// <summary>
    /// Reads the content of a XML file
    /// </summary>
    /// <param name="filePath">The path to the XML file</param>
    /// <returns>The plain text content of the file</returns>
    public string ReadContent(string filePath)
    {
      XDocument xmlDoc = XDocument.Load(filePath);
      string text = xmlDoc.Root!.Value;
      return text;
    }
  }
}
