namespace SearchEngine.Core.Types;

public class ParsedDocument { }

public abstract class Token
{
  public required string Word { get; set; }
  public int Row { get; set; }
  public int Column { get; set; }

  public override string ToString() => $"Row: {Row}, Column: {Column}, Word: '{Word}'";
}

/// DERIVED CLASSES
//For TXT
public class TextToken : Token
{
  public override string ToString() => $"Row: {Row}, Column: {Column}, Word: '{Word}'";
}

// For PDF
public class PdfToken : Token
{
  public int Page { get; set; }

  public override string ToString() =>
    $"Page: {Page}, Row: {Row}, Column: {Column}, Word: '{Word}'";
}

// For DOCX / DOC
public class WordToken : Token
{
  public required string ParagraphId { get; set; } // optional, if structure is important
}

// For Excel
public class ExcelToken : Token
{
  public required string SheetName { get; set; }
  public required string CellReference { get; set; } // like "B2"

  public override string ToString() => $"Sheet: {SheetName}, Cell: {CellReference}, Word: '{Word}'";
}

// For PowerPoint
public class SlideToken : Token
{
  public int SlideNumber { get; set; }
  public required string ShapeId { get; set; } // optional

  public override string ToString() =>
    $"Slide: {SlideNumber}, Row: {Row}, Column: {Column}, Word: '{Word}'";
}

// For HTML/XML
public class HtmlToken : Token
{
  public required string TagName { get; set; }
  public required string XPath { get; set; }
}

public abstract class DocumentMetadata
{
  public required string Author { get; set; }
  public required string FileSize { get; set; }
}

public class TXTDocumentMetaData : DocumentMetadata
{
  public required string LineCount { get; set; } // For TXT
}

public class PDFDocumentMetaData : DocumentMetadata
{
  public required string PageCount { get; set; } // For PDFs
}

public class DOCDocumentMetadata : DocumentMetadata
{
  public required string WordCount { get; set; } // For DOCX
}

public class Document
{
  public required string Id { get; set; }
  public required string Title { get; set; }
  public required string Content { get; set; }
  public required string FilePath { get; set; }
  public required string FileType { get; set; } // "pdf", "docx", "txt"
  public List<string> Keywords { get; set; } = new();
  public List<Token> Tokens { get; set; } = new();
  public required DocumentMetadata Metadata { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? IndexedAt { get; set; }
}
