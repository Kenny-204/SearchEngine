using DocumentFormat.OpenXml.Packaging;
using SearchEngine.DocumentProcessing.Interfaces;
using A = DocumentFormat.OpenXml.Drawing;

namespace SearchEngine.DocumentProcessing.Parsers
{
  /// <summary>
  /// Parser for reading content from PPTX files
  /// </summary>
  public class PptxParser : IParser
  {
    /// <summary>
    /// Reads the content of a PPTX file
    /// </summary>
    /// <param name="filePath">The path to the PPTX file</param>
    /// <returns>The plain text content of the file</returns>
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
     
    }
  }
}
