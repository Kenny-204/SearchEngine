using SearchEngine.Query.Core;
using System;
using SearchEngine.Query.Configuration;
using SearchEngine.Query.Algorithms;
using SearchEngine.Query.Infrastructure;

namespace SearchEngine.Query.Services
{
    /// <summary>
    /// Factory for creating QueryParser instances with different configurations
    /// </summary>
    public static class QueryParserFactory
    {
        /// <summary>
        /// Creates a QueryParser with default configuration
        /// </summary>
        /// <returns>Configured QueryParser instance</returns>
        public static QueryParser CreateDefault()
        {
            var configuration = QueryParserConfiguration.CreateDefault();
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            
            return new QueryParser(stemmingService, stopwordsProvider, configuration);
        }

        /// <summary>
        /// Creates a QueryParser optimized for performance
        /// </summary>
        /// <returns>Performance-optimized QueryParser instance</returns>
        public static QueryParser CreatePerformanceOptimized()
        {
            var configuration = QueryParserConfiguration.CreatePerformanceOptimized();
            var stopwordsProvider = new DefaultStopwordsProvider();
            var cache = new LRUCache<string, string>(1000);
            var stemmingService = new StemmingService(new PorterStemmer(), cache);
            
            return new QueryParser(stemmingService, stopwordsProvider, configuration);
        }

        /// <summary>
        /// Creates a QueryParser optimized for accuracy
        /// </summary>
        /// <returns>Accuracy-optimized QueryParser instance</returns>
        public static QueryParser CreateAccuracyOptimized()
        {
            var configuration = QueryParserConfiguration.CreateAccuracyOptimized();
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            
            return new QueryParser(stemmingService, stopwordsProvider, configuration);
        }

        /// <summary>
        /// Creates a QueryParser with custom configuration
        /// </summary>
        /// <param name="configuration">Custom configuration</param>
        /// <param name="stopwordsProvider">Custom stopwords provider (optional)</param>
        /// <param name="enableCaching">Whether to enable stemming cache</param>
        /// <returns>Custom-configured QueryParser instance</returns>
        public static QueryParser CreateCustom(QueryParserConfiguration configuration, IStopwordsProvider stopwordsProvider = null, bool enableCaching = true)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
                
            stopwordsProvider ??= new DefaultStopwordsProvider();
            
            ICachingService<string, string> cache = null;
            if (enableCaching && configuration.EnableCaching)
            {
                cache = new LRUCache<string, string>(configuration.MaxCacheSize);
            }
            
            var stemmingService = new StemmingService(new PorterStemmer(), cache);
            
            return new QueryParser(stemmingService, stopwordsProvider, configuration);
        }

        /// <summary>
        /// Creates a QueryParser suitable for development and testing
        /// </summary>
        /// <returns>Development-configured QueryParser instance</returns>
        public static QueryParser CreateDevelopment()
        {
            var configuration = new QueryParserConfiguration
            {
                EnableStemming = true,
                RemoveStopwords = true,
                ConvertToLowercase = true,
                MinimumWordLength = 1,
                MaximumWordLength = 50,
                PreserveOriginalFormatting = true,
                TokenizationPattern = @"\W+",
                EnableCaching = true,
                MaxCacheSize = 100
            };
            
            var stopwordsProvider = new DefaultStopwordsProvider();
            var cache = new LRUCache<string, string>(100);
            var stemmingService = new StemmingService(new PorterStemmer(), cache);
            
            return new QueryParser(stemmingService, stopwordsProvider, configuration);
        }
    }
} 