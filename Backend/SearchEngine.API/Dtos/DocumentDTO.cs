namespace SearchEngine.Dtos;

public record class DocumentResponseDto
{
  /// <summary>
  /// Unique document ID.
  /// </summary>
  public required string Id { get; set; }

  /// <summary>
  /// Document title.
  /// </summary>
  public required string Title { get; set; }

  /// <summary>
  /// Cloudinary bucket publicId for the document.
  /// </summary>
  public required string FilePath { get; set; }

  /// <summary>
  /// File extension, e.g., ".pdf".
  /// </summary>
  public required string FileType { get; set; } = string.Empty;

  /// <summary>
  /// List of keywords.
  /// </summary>
  public List<string> Keywords { get; set; } = new();

  /// <summary>
  /// File metadata.
  /// </summary>
  public required FileMetadataDto Metadata { get; set; }

  /// <summary>
  /// File Creation date.
  /// </summary>
  public required DateTime CreatedAt { get; set; }

  /// <summary>
  /// File Index date.
  /// </summary>
  public DateTime? IndexedAt { get; set; }
}

public record class FileMetadataDto
{
  /// <summary>
  /// Author of the document, if available.
  /// </summary>
  public string? Author { get; set; }

  /// <summary>
  /// Page count of the document, if applicable.
  /// </summary>
  public required int PageCount { get; set; }

  /// <summary>
  /// Word count of the document, if applicable.
  /// </summary>
  public required int WordCount { get; set; }

  /// <summary>
  /// File size in bytes.
  /// </summary>
  public required long FileSizeBytes { get; set; }
}
