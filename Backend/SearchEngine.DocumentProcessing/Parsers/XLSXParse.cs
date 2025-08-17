using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  public class Xlsxparser : IParser
  {
    public string ReadContent(string filePath)
    {
      using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
      {
        var workbookPart = document.WorkbookPart!;
        var sharedStringTable = workbookPart.SharedStringTablePart?.SharedStringTable;

        var textBuilder = new System.Text.StringBuilder();

        foreach (var sheet in workbookPart.Workbook.Sheets!.OfType<Sheet>())
        {
          var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
          var rows = worksheetPart!.Worksheet!.GetFirstChild<SheetData>()!.Elements<Row>();

          foreach (var row in rows)
          {
            foreach (var cell in row.Elements<Cell>())
            {
              string cellValue = cell.InnerText;

              // Handle shared string values
              if (cell.DataType?.Value == CellValues.SharedString)
              {
                int index = int.Parse(cellValue);
                cellValue = sharedStringTable!.ElementAt(index).InnerText;
              }

              textBuilder.Append(cellValue + " ");
            }
            textBuilder.AppendLine();
          }
        }

        return textBuilder.ToString();
      }
    }
  }
}
