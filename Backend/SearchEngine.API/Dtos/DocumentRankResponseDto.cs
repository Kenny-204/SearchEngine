using SearchEngine.API.Core;

namespace SearchEngine.Dtos;

public record class DocumentRankResponseDto : DocumentResponseDto
{
  public required double Score { get; set; }
  public required List<WordMatch> Matches { get; set; }
  // /// <summary>
  // /// Unique document ID.
  // /// </summary>
  // public required string Id { get; set; }

  // /// <summary>
  // /// Document title.
  // /// </summary>
  // public required string Title { get; set; }

  // /// <summary>
  // /// Cloudinary bucket publicId for the document.
  // /// </summary>
  // public required string FilePath { get; set; }

  // /// <summary>
  // /// File extension, e.g., ".pdf".
  // /// </summary>
  // public required string FileType { get; set; } = string.Empty;

  // /// <summary>
  // /// List of keywords.
  // /// </summary>
  // public List<string> Keywords { get; set; } = new();

  // /// <summary>
  // /// File metadata.
  // /// </summary>
  // public required FileMetadataDto Metadata { get; set; }

  // /// <summary>
  // /// File Creation date.
  // /// </summary>
  // public required DateTime CreatedAt { get; set; }

  // /// <summary>
  // /// File Index date.
  // /// </summary>
  // public DateTime? IndexedAt { get; set; }
}
