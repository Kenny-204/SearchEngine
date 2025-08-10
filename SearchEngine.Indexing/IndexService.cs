namespace SearchEngine.Indexing;

public interface IDocumentIndexedEventHandler
{
  Task HandleAsync(Guid documentId);
}

public interface IIndexService
{
  Task IndexDocumentAsync(Guid docId);
}

public class IndexService : IIndexService
{
  private readonly IDocumentIndexedEventHandler _eventHandler;

  public IndexService(IDocumentIndexedEventHandler eventHandler)
  {
    _eventHandler = eventHandler;
  }

  public async Task IndexDocumentAsync(Guid docId)
  {
    // Do your indexing logic here...
    await _eventHandler.HandleAsync(docId);
  }
}
