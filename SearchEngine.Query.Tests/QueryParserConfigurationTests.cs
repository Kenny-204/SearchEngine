using System;
using Xunit;
using SearchEngine.Query.Configuration;

namespace SearchEngine.Query.Tests
{
    public class QueryParserConfigurationTests
    {
        [Fact]
        public void CreateDefault_Should_Return_Valid_Configuration()
        {
            var config = QueryParserConfiguration.CreateDefault();
            
            Assert.True(config.EnableStemming);
            Assert.True(config.RemoveStopwords);
            Assert.True(config.ConvertToLowercase);
            Assert.Equal(2, config.MinimumWordLength);
            Assert.Equal(50, config.MaximumWordLength);
            Assert.False(config.PreserveOriginalFormatting);
            Assert.Equal(@"\W+", config.TokenizationPattern);
            Assert.True(config.EnableCaching);
            Assert.Equal(1000, config.MaxCacheSize);
        }
        
        [Fact]
        public void CreatePerformanceOptimized_Should_Return_Performance_Configuration()
        {
            var config = QueryParserConfiguration.CreatePerformanceOptimized();
            
            Assert.True(config.EnableCaching);
            Assert.Equal(5000, config.MaxCacheSize);
            Assert.Equal(3, config.MinimumWordLength);
            Assert.Equal(30, config.MaximumWordLength);
        }
        
        [Fact]
        public void CreateAccuracyOptimized_Should_Return_Accuracy_Configuration()
        {
            var config = QueryParserConfiguration.CreateAccuracyOptimized();
            
            Assert.True(config.EnableStemming);
            Assert.True(config.RemoveStopwords);
            Assert.True(config.ConvertToLowercase);
            Assert.Equal(1, config.MinimumWordLength);
            Assert.Equal(100, config.MaximumWordLength);
            Assert.False(config.EnableCaching);
        }
        
        [Fact]
        public void Validate_Should_Not_Throw_For_Valid_Configuration()
        {
            var config = QueryParserConfiguration.CreateDefault();
            
            // Should not throw
            config.Validate();
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Validate_Should_Throw_For_Negative_MinimumWordLength(int minLength)
        {
            var config = QueryParserConfiguration.CreateDefault();
            config.MinimumWordLength = minLength;
            
            var exception = Assert.Throws<ArgumentException>(() => config.Validate());
            Assert.Contains("MinimumWordLength", exception.Message);
        }
        
        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 5)]
        [InlineData(100, 50)]
        public void Validate_Should_Throw_For_Invalid_MaximumWordLength(int minLength, int maxLength)
        {
            var config = QueryParserConfiguration.CreateDefault();
            config.MinimumWordLength = minLength;
            config.MaximumWordLength = maxLength;
            
            var exception = Assert.Throws<ArgumentException>(() => config.Validate());
            Assert.Contains("MaximumWordLength", exception.Message);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_Should_Throw_For_Invalid_MaxCacheSize(int maxCacheSize)
        {
            var config = QueryParserConfiguration.CreateDefault();
            config.MaxCacheSize = maxCacheSize;
            
            var exception = Assert.Throws<ArgumentException>(() => config.Validate());
            Assert.Contains("MaxCacheSize", exception.Message);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Validate_Should_Throw_For_Invalid_TokenizationPattern(string pattern)
        {
            var config = QueryParserConfiguration.CreateDefault();
            config.TokenizationPattern = pattern;
            
            var exception = Assert.Throws<ArgumentException>(() => config.Validate());
            Assert.Contains("TokenizationPattern", exception.Message);
        }
        
        [Fact]
        public void Configuration_Properties_Should_Be_Settable()
        {
            var config = new QueryParserConfiguration
            {
                EnableStemming = false,
                RemoveStopwords = false,
                ConvertToLowercase = false,
                MinimumWordLength = 5,
                MaximumWordLength = 100,
                PreserveOriginalFormatting = false,
                TokenizationPattern = @"\s+",
                EnableCaching = false,
                MaxCacheSize = 5000
            };
            
            Assert.False(config.EnableStemming);
            Assert.False(config.RemoveStopwords);
            Assert.False(config.ConvertToLowercase);
            Assert.Equal(5, config.MinimumWordLength);
            Assert.Equal(100, config.MaximumWordLength);
            Assert.False(config.PreserveOriginalFormatting);
            Assert.Equal(@"\s+", config.TokenizationPattern);
            Assert.False(config.EnableCaching);
            Assert.Equal(5000, config.MaxCacheSize);
        }
    }
} 