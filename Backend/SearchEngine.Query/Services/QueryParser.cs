using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SearchEngine.Query.Core;
using SearchEngine.Query.Configuration;
using SearchEngine.Query.Algorithms;

namespace SearchEngine.Query.Services
{
    /// <summary>
    /// Parses raw search queries into structured QueryRepresentation objects
    /// </summary>
    public class QueryParser : IQueryParser
    {
        private readonly StemmingService _stemmingService;
        private readonly IStopwordsProvider _stopwordsProvider;
        private readonly QueryParserConfiguration _configuration;
        
        public QueryParser() : this(new StemmingService(new PorterStemmer()))
        {
        }
        
        public QueryParser(StemmingService stemmingService) : this(stemmingService, new DefaultStopwordsProvider(), QueryParserConfiguration.CreateDefault())
        {
        }
        
        public QueryParser(StemmingService stemmingService, IStopwordsProvider stopwordsProvider, QueryParserConfiguration configuration)
        {
            _stemmingService = stemmingService ?? throw new ArgumentNullException(nameof(stemmingService));
            _stopwordsProvider = stopwordsProvider ?? throw new ArgumentNullException(nameof(stopwordsProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            // Validate configuration
            _configuration.Validate();
        }

        public QueryRepresentation Parse(string query)
        {
            // Step 1: Validation
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Query cannot be null, empty, or whitespace only", nameof(query));
            }

            // Step 2: Normalization
            var normalizedQuery = _configuration.ConvertToLowercase 
                ? query.Trim().ToLowerInvariant() 
                : query.Trim();
            
            // Step 3: Replace control characters with spaces
            var cleanedQuery = ReplaceControlCharactersWithSpaces(normalizedQuery);
            
            // Step 4: Tokenization
            var tokens = Regex.Split(cleanedQuery, _configuration.TokenizationPattern)
                .Where(token => !string.IsNullOrEmpty(token))
                .Select(token => token.Trim())
                .Where(token => !string.IsNullOrEmpty(token))
                .Where(token => token.Length >= _configuration.MinimumWordLength && token.Length <= _configuration.MaximumWordLength)
                .ToList();

            // Step 5: Stopword removal (if enabled)
            var filteredTerms = _configuration.RemoveStopwords
                ? tokens.Where(term => !_stopwordsProvider.IsStopword(term)).ToList()
                : tokens;

            // Step 6: Stemming (if enabled)
            var stemmedTerms = _configuration.EnableStemming
                ? _stemmingService.StemTerms(filteredTerms).ToList()
                : filteredTerms;

            // Step 7: Calculate term frequency
            var termFrequency = stemmedTerms.GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count());

            return new QueryRepresentation
            {
                OriginalQuery = query, // Always preserve the original query
                Terms = stemmedTerms,
                HasStopwordsRemoved = _configuration.RemoveStopwords,
                TermFrequency = termFrequency
            };
        }

        private string ReplaceControlCharactersWithSpaces(string input)
        {
            return new string(input.Select(c => char.IsControl(c) ? ' ' : c).ToArray());
        }
    }
} 