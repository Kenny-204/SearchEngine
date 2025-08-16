using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class DocumentModel
{
  [BsonId]
  public ObjectId Id { get; set; }

  [BsonElement("title")]
  public string Title { get; set; } = string.Empty;

  [BsonElement("filePath")]
  public string FilePath { get; set; } = string.Empty;

  [BsonElement("fileType")]
  public string FileType { get; set; } = string.Empty;

  [BsonElement("keywords")]
  public List<string> Keywords { get; set; } = new();

  [BsonElement("metadata")]
  public required FileMetadata Metadata { get; set; }

  [BsonElement("createdAt")]
  [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  [BsonElement("indexedAt")]
  [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
  public DateTime? IndexedAt { get; set; }
}

public class FileMetadata
{
  [BsonElement("author")]
  public string? Author { get; set; }

  [BsonElement("pageCount")]
  public int PageCount { get; set; }

  [BsonElement("wordCount")]
  public int WordCount { get; set; }

  [BsonElement("fileSize")]
  public long FileSize { get; set; }
}
