using MongoDB.Driver;

public class MongoDbContext
{
  private readonly IMongoDatabase _database;

  public MongoDbContext(string connectionString)
  {
    var client = new MongoClient(connectionString);
    _database = client.GetDatabase("dev");
  }

  public IMongoCollection<DocumentModel> Documents =>
    _database.GetCollection<DocumentModel>("Documents");
  public IMongoCollection<InvertedIndexTerm> InvertedIndex =>
    _database.GetCollection<InvertedIndexTerm>("InvertedIndex");
}
