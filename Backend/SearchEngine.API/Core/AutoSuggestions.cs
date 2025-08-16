using MongoDB.Driver;
using SearchEngine.Services;

namespace SearchEngine.API.Core;

public class AutoSuggestion
{
  private readonly MongoDbContext _db;
  private readonly RedisCacheService _cache;

  public AutoSuggestion(MongoDbContext db, RedisCacheService cache)
  {
    _db = db;
    _cache = cache;
  }

  public async Task<List<string>> SuggestAsync(string prefix)
  {
    var cacheKey = $"autocomplete:{prefix.ToLower()}";
    var cached = await _cache.GetAsync<List<string>>(cacheKey);
    var dbTerms = cached;
    if (dbTerms is null)
    {
      // 1. Query your MongoDB inverted index for terms starting with prefix
      var nextPrefix = prefix.Substring(0, prefix.Length - 1) + (char)(prefix[^1] + 1);

      var filter = Builders<InvertedIndexTerm>.Filter.And(
        Builders<InvertedIndexTerm>.Filter.Gte("_id", prefix),
        Builders<InvertedIndexTerm>.Filter.Lt("_id", nextPrefix)
      );

      dbTerms = await _db
        .InvertedIndex.Find(filter)
        .Sort(Builders<InvertedIndexTerm>.Sort.Ascending("_id"))
        .Limit(10)
        .Project(t => t.Term)
        .ToListAsync();
      await _cache.SetAsync(cacheKey, dbTerms, TimeSpan.FromMinutes(10));
    }

    // 2. Filter in-memory stop words by prefix (case insensitive)
    var stopWords = Globals
      .StopWords.Where(sw => sw.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
      .OrderBy(sw => sw)
      .Take(10)
      .ToList();

    // 3. Merge, deduplicate, and limit total results to 10 (or any limit you want)
    var combined = dbTerms
      .Concat(stopWords)
      .Distinct(StringComparer.OrdinalIgnoreCase)
      .OrderBy(term => term.Length) // Sort by length first
      .ThenBy(term => term) // Then alphabetical for same length
      .Take(10)
      .ToList();

    return combined;
  }
}
