using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Parsers
{
  /// <summary>
  /// Parser for reading content from XLSX files
  /// </summary>
  public class Xlsxparser : IParser
  {
    /// <summary>
    /// Reads the content of a XLSX file
    /// </summary>
    /// <param name="filePath">The path to the XLSX file</param>
    /// <returns>The plain text content of the file</returns>
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
