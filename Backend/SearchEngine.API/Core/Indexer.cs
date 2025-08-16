using DocumentFormat.OpenXml.Wordprocessing;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SearchEngine.API.Core;

public class Indexer
{
  private readonly MongoDbContext _db;

  public Indexer(MongoDbContext db)
  {
    _db = db;
  }

  public async Task UpdateInvertedIndexAsync(
    Dictionary<string, int> termFrequencies,
    ObjectId docId
  )
  {
    foreach (var kvp in termFrequencies)
    {
      var term = kvp.Key;
      var tf = kvp.Value;

      // Try update existing term first
      var filter = Builders<InvertedIndexTerm>.Filter.Eq(t => t.Term, term);
      var existingTerm = await _db.InvertedIndex.Find(filter).FirstOrDefaultAsync();

      if (existingTerm != null)
      {
        // Check if doc already in postings
        var posting = existingTerm.Postings.FirstOrDefault(p => p.DocId == docId);
        if (posting != null)
        {
          posting.TermFrequency = tf;
        }
        else
        {
          existingTerm.Postings.Add(new Posting { DocId = docId, TermFrequency = tf });
          existingTerm.DocumentFrequency++;
        }

        existingTerm.TotalOccurrences = existingTerm.Postings.Sum(p => p.TermFrequency);
        existingTerm.LastUpdated = DateTime.UtcNow;

        await _db.InvertedIndex.ReplaceOneAsync(filter, existingTerm);
      }
      else
      {
        // Create new term entry
        var newTerm = new InvertedIndexTerm
        {
          Term = term,
          Postings = new List<Posting>
          {
            new Posting { DocId = docId, TermFrequency = tf },
          },
          DocumentFrequency = 1,
          TotalOccurrences = tf,
          LastUpdated = DateTime.UtcNow,
        };

        await _db.InvertedIndex.InsertOneAsync(newTerm);
      }
    }
  }

  public async Task BatchInsertInvertedIndexAsync(
    List<(ObjectId DocId, Dictionary<string, int> TermFrequencies)> documents
  )
  {
    var bulkOps = new List<WriteModel<InvertedIndexTerm>>();

    foreach (var (docId, termFreqs) in documents)
    {
      foreach (var kvp in termFreqs)
      {
        var term = kvp.Key;
        var tf = kvp.Value;

        var filter = Builders<InvertedIndexTerm>.Filter.Eq(t => t.Term, term);

        var update = Builders<InvertedIndexTerm>
          .Update.SetOnInsert(t => t.Term, term)
          .Set("last_updated", DateTime.UtcNow)
          .Inc("df", 1) // new document for this term
          .Inc("total_occurrences", tf)
          .Push("postings", new Posting { DocId = docId, TermFrequency = tf });

        var upsert = new UpdateOneModel<InvertedIndexTerm>(filter, update) { IsUpsert = true };

        bulkOps.Add(upsert);
      }
    }

    if (bulkOps.Count > 0)
    {
      await _db.InvertedIndex.BulkWriteAsync(bulkOps, new BulkWriteOptions { IsOrdered = false });
    }
  }

  public async Task BatchUpdateGroupedInvertedIndexAsync(
    List<(ObjectId DocId, Dictionary<string, int> TermFrequencies)> documents
  )
  {
    // Step 1: Group all postings by term
    var groupedTerms = new Dictionary<string, List<Posting>>();

    foreach (var (docId, termFreqs) in documents)
    {
      foreach (var kvp in termFreqs)
      {
        var term = kvp.Key;
        var tf = kvp.Value;

        if (!groupedTerms.ContainsKey(term))
          groupedTerms[term] = new List<Posting>();

        groupedTerms[term].Add(new Posting { DocId = docId, TermFrequency = tf });
      }
    }

    // Step 2: Create one bulk update per unique term
    var bulkOps = new List<WriteModel<InvertedIndexTerm>>();

    foreach (var kvp in groupedTerms)
    {
      var term = kvp.Key;
      var postings = kvp.Value;

      var totalOccurrences = postings.Sum(p => p.TermFrequency);
      var df = postings.Count; // number of docs in this batch for this term

      var filter = Builders<InvertedIndexTerm>.Filter.Eq(t => t.Term, term);

      var update = Builders<InvertedIndexTerm>
        .Update.SetOnInsert(t => t.Term, term)
        .Set("last_updated", DateTime.UtcNow)
        .Inc("df", df)
        .Inc("total_occurrences", totalOccurrences)
        .PushEach("postings", postings); // push all postings in one go

      var upsert = new UpdateOneModel<InvertedIndexTerm>(filter, update) { IsUpsert = true };

      bulkOps.Add(upsert);
    }

    // Step 3: Execute all updates in one bulk call
    if (bulkOps.Count > 0)
    {
      await _db.InvertedIndex.BulkWriteAsync(bulkOps, new BulkWriteOptions { IsOrdered = false });
    }

    // Step 4: Update indexedAt for each processed document
    await UpdateIndexedAt(documents.Select(d => d.DocId));
  }

  public async Task UpdateIndexedAt(IEnumerable<ObjectId> documents)
  {
    var docUpdateOps = new List<WriteModel<DocumentModel>>();
    var now = DateTime.UtcNow;

    foreach (var docId in documents)
    {
      var filter = Builders<DocumentModel>.Filter.Eq(d => d.Id, docId);
      var update = Builders<DocumentModel>.Update.Set("indexedAt", now);

      docUpdateOps.Add(new UpdateOneModel<DocumentModel>(filter, update));
    }

    if (docUpdateOps.Count > 0)
    {
      await _db.Documents.BulkWriteAsync(docUpdateOps, new BulkWriteOptions { IsOrdered = false });
    }
  }
}
