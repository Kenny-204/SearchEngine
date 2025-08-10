using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchEngine.Query
{
    public class QueryParser : IQueryParser
    {
        private readonly HashSet<string> _stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with", "by"
        };

        public QueryRepresentation Parse(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new QueryRepresentation
                {
                    OriginalQuery = query ?? string.Empty,
                    Terms = new List<string>(),
                    HasStopwordsRemoved = false,
                    TermFrequency = new Dictionary<string, int>()
                };
            }

            // Step 1: Normalize - convert to lowercase and trim
            var normalizedQuery = query.Trim().ToLowerInvariant();
            
            // Step 2: Replace control characters with spaces to treat them as separators
            var cleanedQuery = ReplaceControlCharactersWithSpaces(normalizedQuery);
            
            // Step 3: Tokenize using regex to split on non-word characters
            var tokens = Regex.Split(cleanedQuery, @"\W+")
                .Where(token => !string.IsNullOrEmpty(token))
                .Select(token => token.Trim())
                .Where(token => !string.IsNullOrEmpty(token))
                .ToList();

            // Step 4: Remove stopwords
            var filteredTerms = tokens
                .Where(term => !_stopWords.Contains(term))
                .ToList();

            // Step 5: Calculate term frequency
            var termFrequency = filteredTerms.GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count());

            return new QueryRepresentation 
            { 
                OriginalQuery = query,
                Terms = filteredTerms,
                HasStopwordsRemoved = true,
                TermFrequency = termFrequency
            };
        }

        private string ReplaceControlCharactersWithSpaces(string input)
        {
            return new string(input.Select(c => char.IsControl(c) ? ' ' : c).ToArray());
        }
    }
} 