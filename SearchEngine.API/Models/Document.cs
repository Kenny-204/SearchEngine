using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Document
{
  [BsonId] // Marks this as the primary key
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }

  [BsonElement("username")]
  public string Username { get; set; }
}
