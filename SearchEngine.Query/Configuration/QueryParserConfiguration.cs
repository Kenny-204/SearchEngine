using System;

namespace SearchEngine.Query.Configuration
{
    /// <summary>
    /// Configuration class for QueryParser behavior and settings
    /// </summary>
    public class QueryParserConfiguration
    {
        /// <summary>
        /// Whether to enable stemming of terms
        /// </summary>
        public bool EnableStemming { get; set; } = true;

        /// <summary>
        /// Whether to remove stopwords from queries
        /// </summary>
        public bool RemoveStopwords { get; set; } = true;

        /// <summary>
        /// Whether to convert all text to lowercase
        /// </summary>
        public bool ConvertToLowercase { get; set; } = true;

        /// <summary>
        /// Minimum word length to include in parsing
        /// </summary>
        public int MinimumWordLength { get; set; } = 2;

        /// <summary>
        /// Maximum word length to include in parsing
        /// </summary>
        public int MaximumWordLength { get; set; } = 50;

        /// <summary>
        /// Whether to preserve original query formatting in output
        /// </summary>
        public bool PreserveOriginalFormatting { get; set; } = false;

        /// <summary>
        /// Regex pattern for tokenization
        /// </summary>
        public string TokenizationPattern { get; set; } = @"\W+";

        /// <summary>
        /// Whether to enable caching for stemming operations
        /// </summary>
        public bool EnableCaching { get; set; } = true;

        /// <summary>
        /// Maximum size of the stemming cache
        /// </summary>
        public int MaxCacheSize { get; set; } = 1000;

        /// <summary>
        /// Creates a default configuration
        /// </summary>
        /// <returns>Default QueryParserConfiguration</returns>
        public static QueryParserConfiguration CreateDefault()
        {
            return new QueryParserConfiguration();
        }

        /// <summary>
        /// Creates a configuration optimized for performance
        /// </summary>
        /// <returns>Performance-optimized QueryParserConfiguration</returns>
        public static QueryParserConfiguration CreatePerformanceOptimized()
        {
            return new QueryParserConfiguration
            {
                EnableStemming = true,
                RemoveStopwords = true,
                ConvertToLowercase = true,
                MinimumWordLength = 3,
                MaximumWordLength = 30,
                PreserveOriginalFormatting = false,
                TokenizationPattern = @"\W+",
                EnableCaching = true,
                MaxCacheSize = 5000
            };
        }

        /// <summary>
        /// Creates a configuration optimized for accuracy
        /// </summary>
        /// <returns>Accuracy-optimized QueryParserConfiguration</returns>
        public static QueryParserConfiguration CreateAccuracyOptimized()
        {
            return new QueryParserConfiguration
            {
                EnableStemming = true,
                RemoveStopwords = true,
                ConvertToLowercase = true,
                MinimumWordLength = 1,
                MaximumWordLength = 100,
                PreserveOriginalFormatting = true,
                TokenizationPattern = @"\W+",
                EnableCaching = false,
                MaxCacheSize = 1000
            };
        }

        /// <summary>
        /// Validates the configuration settings
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when configuration is invalid</exception>
        public void Validate()
        {
            if (MinimumWordLength < 0)
                throw new ArgumentException("MinimumWordLength cannot be negative", nameof(MinimumWordLength));

            if (MaximumWordLength <= 0)
                throw new ArgumentException("MaximumWordLength must be positive", nameof(MaximumWordLength));

            if (MinimumWordLength > MaximumWordLength)
                throw new ArgumentException("MinimumWordLength cannot be greater than MaximumWordLength", nameof(MinimumWordLength));

            if (MaxCacheSize <= 0)
                throw new ArgumentException("MaxCacheSize must be positive", nameof(MaxCacheSize));

            if (string.IsNullOrWhiteSpace(TokenizationPattern))
                throw new ArgumentException("TokenizationPattern cannot be null, empty, or whitespace only", nameof(TokenizationPattern));
        }
    }
} 