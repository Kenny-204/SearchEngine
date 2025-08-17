using DocumentFormat.OpenXml.Packaging;
using SearchEngine.DocumentProcessing.Interfaces;
using A = DocumentFormat.OpenXml.Drawing;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class PptxParser : IParser
  {
    public string ReadContent(string filePath)
    {
      using (PresentationDocument presentation = PresentationDocument.Open(filePath, false))
      {
        var textBuilder = new System.Text.StringBuilder();

        var slideParts = presentation!.PresentationPart!.SlideParts;

        foreach (var slidePart in slideParts)
        {
          var shapes = slidePart.Slide.Descendants<A.Text>();
          foreach (var text in shapes)
          {
            textBuilder.AppendLine(text.Text);
          }
        }

        return textBuilder.ToString();
      }
      throw new NotImplementedException();
    }
  }
}
