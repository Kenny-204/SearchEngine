using MongoDB.Driver;
using SearchEngine.Query.Core;
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

  public async Task<List<InvertedIndexTerm>> AutoCompleteQuery(QueryRepresentation query)
  {
    var cacheKey = $"autocomplete_query:{query.OriginalQuery.ToLower()}";
    var cached = await _cache.GetAsync<List<InvertedIndexTerm>>(cacheKey);
    if (cached is not null)
    {
      return cached;
    }
    Console.WriteLine("sdscsvsv");
    var prefixes = query.Terms;

    // 1️⃣ Normalize and deduplicate
    var uniquePrefixes = prefixes
      .Where(p => !string.IsNullOrWhiteSpace(p))
      .Select(p => p.Trim())
      .Distinct(StringComparer.OrdinalIgnoreCase)
      .OrderBy(p => p, StringComparer.OrdinalIgnoreCase) // important for prefix check
      .ToList();

    // 2️⃣ Remove redundant prefixes (e.g., "car" is inside "ca")
    var minimalPrefixes = new List<string>();
    foreach (var prefix in uniquePrefixes)
    {
      if (
        !minimalPrefixes.Any(existing =>
          prefix.StartsWith(existing, StringComparison.OrdinalIgnoreCase)
        )
      )
      {
        minimalPrefixes.Add(prefix);
      }
    }
    // 3️⃣ Build filters only from minimal set
    var filters = new List<FilterDefinition<InvertedIndexTerm>>();
    foreach (var prefix in minimalPrefixes)
    {
      var nextPrefix = prefix.Substring(0, prefix.Length - 1) + (char)(prefix[^1] + 1);

      var rangeFilter = Builders<InvertedIndexTerm>.Filter.And(
        Builders<InvertedIndexTerm>.Filter.Gte("_id", prefix),
        Builders<InvertedIndexTerm>.Filter.Lt("_id", nextPrefix)
      );

      filters.Add(rangeFilter);
    }

    if (filters is null || filters.Count == 0)
      return [];

    var finalFilter = Builders<InvertedIndexTerm>.Filter.Or(filters);

    var dbTerms = await _db
      .InvertedIndex.Find(finalFilter)
      // .Limit(10) // not still sure i should limit or not
      .ToListAsync();

    await _cache.SetAsync(cacheKey, dbTerms, TimeSpan.FromMinutes(2));
    Globals.Print(dbTerms.Select(s => s.Term));
    return dbTerms;
  }
}
