using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Posting
{
  [BsonElement("docId")]
  public ObjectId DocId { get; set; }

  [BsonElement("tf")]
  public int TermFrequency { get; set; }
}

public class InvertedIndexTerm
{
  [BsonId] // Marks this as the primary key
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
