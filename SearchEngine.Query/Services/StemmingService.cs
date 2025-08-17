using System;
using System.Collections.Generic;
using System.Linq;
using SearchEngine.Query.Algorithms;
using SearchEngine.Query.Core;

namespace SearchEngine.Query.Services
{
  /// <summary>
  /// Service for stemming words using the Porter Stemmer algorithm
  /// </summary>
  public class StemmingService
  {
    private readonly IPorterStemmer _stemmer;
    private readonly ICachingService<string, string> _cache;

    public StemmingService(IPorterStemmer stemmer, ICachingService<string, string> cache = null)
    {
      _stemmer = stemmer ?? throw new System.ArgumentNullException(nameof(stemmer));
      _cache = cache;
    }

    /// <summary>
    /// Stems a single term
    /// </summary>
    /// <param name="term">The term to stem</param>
    /// <returns>The stemmed term</returns>
    public string StemTerm(string term)
    {
      if (string.IsNullOrEmpty(term))
        return term;

      if (!ShouldStem(term))
        return term;

      // Try to get from cache first
      if (_cache != null && _cache.TryGet(term, out var cachedStem))
      {
        return cachedStem;
      }

      // Use the Porter Stemmer for complex cases
      var stemmed = _stemmer.Stem(term);

      // Cache the result
      if (_cache != null)
      {
        _cache.Set(term, stemmed);
      }

      return stemmed;
    }

    /// <summary>
    /// Stems multiple terms
    /// </summary>
    /// <param name="terms">Collection of terms to stem</param>
    /// <returns>Collection of stemmed terms</returns>
    public IEnumerable<string> StemTerms(IEnumerable<string> terms)
    {
      if (terms == null)
        return Enumerable.Empty<string>();

      return terms.Select(StemTerm);
    }

    /// <summary>
    /// Determines if a term should be stemmed
    /// </summary>
    /// <param name="term">The term to evaluate</param>
    /// <returns>True if the term should be stemmed, false otherwise</returns>
    private bool ShouldStem(string term)
    {
      if (string.IsNullOrEmpty(term) || term.Length < 3)
        return false;

      // Stem words that are 3+ characters and have common suffixes
      var lowerTerm = term.ToLowerInvariant();
      var suffixes = new[]
      {
        "s",
        "ing",
        "ed",
        "er",
        "est",
        "ly",
        "al",
        "ation",
        "ness",
        "ment",
        "ful",
        "less",
      };

      return suffixes.Any(suffix => lowerTerm.EndsWith(suffix));
    }
  }
}