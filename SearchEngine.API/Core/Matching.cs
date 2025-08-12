using MongoDB.Bson;
using MongoDB.Driver;
using SearchEngine.Query.Core;
using SearchEngine.Query.Services;

namespace SearchEngine.API.Core;

public class DocumentsScore
{
  public required ObjectId Id { get; set; }
  public required double Score { get; set; }
}

public class DocMatcher
{
  private readonly MongoDbContext _db;
  private readonly AutoSuggestion _autosuggest;
  private readonly QueryParser _queryParser;

  public DocMatcher(MongoDbContext db, AutoSuggestion autosuggest, QueryParser queryParser)
  {
    _db = db;
    _autosuggest = autosuggest;
    _queryParser = queryParser;
  }

  private IEnumerable<DocumentsScore> TFIDF(List<InvertedIndexTerm> terms, long N)
  {
    List<KeyValuePair<ObjectId, double>> allScores = [];

    foreach (var term in terms)
    {
      var df = term.DocumentFrequency;

      var idf = Math.Log10(N / df);
      foreach (var posting in term.Postings)
      {
        var tf = posting.TermFrequency;
        allScores.Add(new KeyValuePair<ObjectId, double>(posting.DocId, tf * idf));
      }
    }

    IEnumerable<DocumentsScore> scores = allScores
      .GroupBy(g => g.Key)
      .Select(g => new DocumentsScore() { Id = g.Key, Score = g.Sum(s => s.Value) })
      .OrderByDescending(x => x.Score);

    Globals.Print(scores.Select(s => s.Score));

    return scores;
  }

  private async Task<List<InvertedIndexTerm>> MatchedTerms(QueryRepresentation query)
  {
    List<InvertedIndexTerm> terms = await _autosuggest.AutoCompleteQuery(query);
    return terms;
  }

  public async Task<List<DocumentsScore>> MatchQueryAsync(string input)
  {
    long count = await _db.Documents.CountDocumentsAsync(FilterDefinition<DocumentModel>.Empty);
    Console.WriteLine($"Count is {count}");
    var query = _queryParser.Parse(input);
    Globals.Print(query.Terms);
    var terms = await MatchedTerms(query);
    var documents = TFIDF(terms, count).ToList();
    return documents;
  }
}
