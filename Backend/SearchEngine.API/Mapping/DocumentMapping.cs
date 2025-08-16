using System.Reflection.Metadata;
using SearchEngine.Dtos;

namespace SearchEngine.Mappings;

public static class DocumentMapping
{
  public static DocumentResponseDto ToDto(DocumentModel model)
  {
    var meta = new FileMetadataDto()
    {
      Author = model.Metadata.Author,
      PageCount = model.Metadata.PageCount,
      WordCount = model.Metadata.WordCount,
      FileSizeBytes = model.Metadata.FileSize,
    };
    return new DocumentResponseDto()
    {
      Id = model.Id.ToString(),
      Title = model.Title,
      FilePath = model.FilePath,
      FileType = model.FileType,
      Keywords = model.Keywords,
      Metadata = meta,
      CreatedAt = model.CreatedAt,
      IndexedAt = model.IndexedAt,
    };
  }
}
