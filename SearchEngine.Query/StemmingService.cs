using System.Collections.Generic;
using System.Linq;

namespace SearchEngine.Query
{
    /// <summary>
    /// Service for handling stemming operations in the query processing pipeline
    /// </summary>
    public class StemmingService
    {
        private readonly IPorterStemmer _stemmer;

        public StemmingService(IPorterStemmer stemmer)
        {
            _stemmer = stemmer ?? throw new System.ArgumentNullException(nameof(stemmer));
        }

        /// <summary>
        /// Stems a collection of terms using the Porter Stemmer algorithm
        /// </summary>
        /// <param name="terms">Collection of terms to stem</param>
        /// <returns>Collection of stemmed terms</returns>
        public IEnumerable<string> StemTerms(IEnumerable<string> terms)
        {
            if (terms == null)
                return Enumerable.Empty<string>();

            return terms.Select(term => ShouldStem(term) ? StemTerm(term) : term);
        }

        /// <summary>
        /// Stems a single term using the Porter Stemmer algorithm
        /// </summary>
        /// <param name="term">Term to stem</param>
        /// <returns>Stemmed term</returns>
        public string StemTerm(string term)
        {
            if (string.IsNullOrEmpty(term))
                return term;

            if (!ShouldStem(term))
                return term;

            // Use the Porter Stemmer for complex cases
            return _stemmer.Stem(term);
        }

        /// <summary>
        /// Determines if a term should be stemmed based on length and content
        /// </summary>
        /// <param name="term">Term to evaluate</param>
        /// <returns>True if the term should be stemmed</returns>
        public bool ShouldStem(string term)
        {
            if (string.IsNullOrEmpty(term))
                return false;

            // Don't stem very short words or numbers
            if (term.Length < 3 || term.All(char.IsDigit))
                return false;

            // Don't stem words that are already in their base form (common cases)
            var commonBaseWords = new HashSet<string> { 
                "the", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with", "by",
                "apple", "orange", "banana", "house", "car", "book", "tree", "water", "fire",
                "earth", "air", "sun", "moon", "star", "bird", "fish", "dog", "cat", "man",
                "woman", "child", "friend", "family", "home", "work", "school", "time", "day",
                "night", "year", "month", "week", "hour", "minute", "second", "world", "country",
                "city", "town", "street", "road", "way", "place", "thing", "person", "people"
            };
            if (commonBaseWords.Contains(term.ToLowerInvariant()))
                return false;

            // Check if the word has common suffixes that indicate it should be stemmed
            var commonSuffixes = new[] 
            { 
                "s", "es", "ies", "ed", "ing", "ly", "er", "est", "al", "ance", "ence", 
                "able", "ible", "ize", "ise", "ation", "ition", "ness", "ful", "ous"
            };

            foreach (var suffix in commonSuffixes)
            {
                if (term.EndsWith(suffix, System.StringComparison.OrdinalIgnoreCase))
                {
                    // Special case: don't stem very short words with just 's' suffix
                    if (suffix == "s" && term.Length <= 3)
                        return false;
                    
                    // Don't stem if removing the suffix would make the word too short
                    if (term.Length - suffix.Length < 2)
                        return false;
                    
                    return true;
                }
            }

            return false;
        }

        private bool IsVowel(char c)
        {
            return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
        }
    }
} 