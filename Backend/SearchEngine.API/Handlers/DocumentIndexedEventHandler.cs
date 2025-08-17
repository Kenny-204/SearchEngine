using SearchEngine.Indexing;

public class DocumentIndexedEventHandler : IDocumentIndexedEventHandler
{
  private readonly MongoDbContext _db;

  public DocumentIndexedEventHandler(MongoDbContext db)
  {
    _db = db;
  }

  public async Task HandleAsync(Guid documentId)
  {
    // var doc = await _db.Documents.FindAsync(documentId);
    // if (doc != null)
    // {
    //     doc.IsIndexed = true;
    //     await _db.SaveChangesAsync();
    // }
  }
}
