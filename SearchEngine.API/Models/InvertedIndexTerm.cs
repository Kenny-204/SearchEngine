using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class TokenPosition
{
  [BsonElement("page")]
  public int Page { get; set; }

  [BsonElement("row")]
  public int Row { get; set; }

  [BsonElement("column")]
  public int Column { get; set; }
}

public class Posting
{
  [BsonElement("docId")]
  public string DocId { get; set; } = string.Empty;

  [BsonElement("positions")]
  public List<TokenPosition> Positions { get; set; } = new();
}

public class InvertedIndexTerm
{
  [BsonId] // Marks this as the primary key
  [BsonRepresentation(BsonType.ObjectId)]
  public required string Term { get; set; }

  [BsonElement("postings")]
  public List<Posting> Postings { get; set; } = new();

  [BsonElement("df")]
  public int DocumentFrequency { get; set; }

  [BsonElement("total_occurrences")]
  public int TotalOccurrences { get; set; }

  [BsonElement("last_updated")]
  public DateTime LastUpdated { get; set; }
}
