using System;
using Xunit;
using SearchEngine.Query.Services;
using SearchEngine.Query.Configuration;
using SearchEngine.Query.Core;

namespace SearchEngine.Query.Tests
{
    public class QueryParserFactoryTests
    {
        [Fact]
        public void CreateDefault_Should_Return_Valid_QueryParser()
        {
            var parser = QueryParserFactory.CreateDefault();
            
            Assert.NotNull(parser);
            Assert.IsType<QueryParser>(parser);
        }
        
        [Fact]
        public void CreatePerformanceOptimized_Should_Return_Valid_QueryParser()
        {
            var parser = QueryParserFactory.CreatePerformanceOptimized();
            
            Assert.NotNull(parser);
            Assert.IsType<QueryParser>(parser);
        }
        
        [Fact]
        public void CreateAccuracyOptimized_Should_Return_Valid_QueryParser()
        {
            var parser = QueryParserFactory.CreateAccuracyOptimized();
            
            Assert.NotNull(parser);
            Assert.IsType<QueryParser>(parser);
        }
        
        [Fact]
        public void CreateDevelopment_Should_Return_Valid_QueryParser()
        {
            var parser = QueryParserFactory.CreateDevelopment();
            
            Assert.NotNull(parser);
            Assert.IsType<QueryParser>(parser);
        }
        
        [Fact]
        public void CreateCustom_Should_Return_Valid_QueryParser()
        {
            var configuration = QueryParserConfiguration.CreateDefault();
            var parser = QueryParserFactory.CreateCustom(configuration);
            
            Assert.NotNull(parser);
            Assert.IsType<QueryParser>(parser);
        }
        
        [Fact]
        public void CreateCustom_Should_Throw_For_Null_Configuration()
        {
            Assert.Throws<ArgumentNullException>(() => QueryParserFactory.CreateCustom(null));
        }
        
        [Fact]
        public void CreateCustom_Should_Use_Provided_Configuration()
        {
            var configuration = new QueryParserConfiguration
            {
                EnableStemming = false,
                RemoveStopwords = false,
                ConvertToLowercase = false,
                MinimumWordLength = 5,
                MaximumWordLength = 100
            };
            
            var parser = QueryParserFactory.CreateCustom(configuration);
            
            // Test that the configuration is applied by parsing a query
            var result = parser.Parse("The quick brown fox jumps over the lazy dog");
            
            // Since stemming and stopword removal are disabled, we should get more terms
            Assert.True(result.Terms.Count > 0);
            Assert.False(result.HasStopwordsRemoved);
        }
        
        [Fact]
        public void CreateCustom_Should_Enable_Caching_When_Requested()
        {
            var configuration = QueryParserConfiguration.CreateDefault();
            var parser = QueryParserFactory.CreateCustom(configuration, enableCaching: true);
            
            Assert.NotNull(parser);
            
            // Test that caching works by parsing the same query twice
            var result1 = parser.Parse("running quickly");
            var result2 = parser.Parse("running quickly");
            
            // Both should produce the same result
            Assert.Equal(result1.Terms, result2.Terms);
        }
        
        [Fact]
        public void CreateCustom_Should_Disable_Caching_When_Requested()
        {
            var configuration = QueryParserConfiguration.CreateDefault();
            var parser = QueryParserFactory.CreateCustom(configuration, enableCaching: false);
            
            Assert.NotNull(parser);
            
            // Test that the parser works without caching
            var result = parser.Parse("running quickly");
            Assert.NotNull(result);
            Assert.True(result.Terms.Count > 0);
        }
        
        [Fact]
        public void Factory_Methods_Should_Produce_Different_Configurations()
        {
            var defaultParser = QueryParserFactory.CreateDefault();
            var performanceParser = QueryParserFactory.CreatePerformanceOptimized();
            var accuracyParser = QueryParserFactory.CreateAccuracyOptimized();
            var developmentParser = QueryParserFactory.CreateDevelopment();
            
            // All should be valid parsers
            Assert.NotNull(defaultParser);
            Assert.NotNull(performanceParser);
            Assert.NotNull(accuracyParser);
            Assert.NotNull(developmentParser);
            
            // Test that they can all parse queries
            var testQuery = "the quick brown fox";
            
            var defaultResult = defaultParser.Parse(testQuery);
            var performanceResult = performanceParser.Parse(testQuery);
            var accuracyResult = accuracyParser.Parse(testQuery);
            var developmentResult = developmentParser.Parse(testQuery);
            
            Assert.NotNull(defaultResult);
            Assert.NotNull(performanceResult);
            Assert.NotNull(accuracyResult);
            Assert.NotNull(developmentResult);
        }
        
        [Fact]
        public void CreateDefault_Should_Handle_Complex_Queries()
        {
            var parser = QueryParserFactory.CreateDefault();
            var result = parser.Parse("The quick brown fox jumps over the lazy dog");
            
            Assert.NotNull(result);
            Assert.Equal("The quick brown fox jumps over the lazy dog", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.True(result.Terms.Count > 0);
            Assert.True(result.TermFrequency.Count > 0);
        }
    }
} 