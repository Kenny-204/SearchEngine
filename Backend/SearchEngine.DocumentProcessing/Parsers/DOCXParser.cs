using DocumentFormat.OpenXml.Packaging;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class DocxParser : IParser
  {
    public string ReadContent(string filePath)
    {
      using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
      {
        var body = wordDoc?.MainDocumentPart?.Document?.Body;
        return body!.InnerText;
      }

      throw new NotImplementedException();
    }
  }
}
