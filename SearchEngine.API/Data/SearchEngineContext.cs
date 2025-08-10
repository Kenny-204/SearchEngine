using dotenv.net;
using MongoDB.Driver;

public class MongoDbContext
{
  private readonly IMongoDatabase _database;

  public MongoDbContext(IConfiguration config)
  {
    var envVars = DotEnv.Read();
    var client = new MongoClient(envVars["MONGODB_CONNECTION_STRING"]);
    _database = client.GetDatabase("dev");
  }

  public IMongoCollection<DocumentModel> Documents => _database.GetCollection<DocumentModel>("Documents");
  public IMongoCollection<InvertedIndexTerm> InvertedIndex =>
    _database.GetCollection<InvertedIndexTerm>("InvertedIndex");
}
