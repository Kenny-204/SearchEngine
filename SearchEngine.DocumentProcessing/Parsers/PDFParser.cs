using System.Linq;
using System.Text;
using SearchEngine.DocumentProcessing.Interfaces;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Graphics.Operations.TextState;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class PDFParser : IParser
  {
    public StringBuilder text = new StringBuilder();

    public string ReadContent(string filePath)
    {
      using (PdfDocument document = PdfDocument.Open(filePath))
      {
        foreach (Page page in document.GetPages())
        {
          IEnumerable<Word> words = page.GetWords();
          text.Append(string.Join(" ", words.Select(x => x.Text)));
        }
      }

      return text.ToString();
    }
  }
}
