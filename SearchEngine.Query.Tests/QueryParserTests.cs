using Xunit;
using SearchEngine.Query.Core;
using SearchEngine.Query.Services;
using SearchEngine.Query.Configuration;
using SearchEngine.Query.Algorithms;
using System;
using System.Linq;

namespace SearchEngine.Query.Tests
{
    public class QueryParserTests
    {
        private readonly IQueryParser _parser;

        public QueryParserTests()
        {
            // Use a parser with configuration that allows single-character words and preserves original formatting
            var configuration = new QueryParserConfiguration
            {
                MinimumWordLength = 1,
                MaximumWordLength = 50,
                PreserveOriginalFormatting = true
            };
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            _parser = new QueryParser(stemmingService, stopwordsProvider, configuration);
        }

        [Fact]
        public void Parse_Should_Return_QueryRepresentation_With_Correct_Structure()
        {
            var result = _parser.Parse("test query");

            Assert.NotNull(result);
            Assert.IsType<QueryRepresentation>(result);
            Assert.Equal("test query", result.OriginalQuery);
            Assert.NotNull(result.Terms);
            Assert.NotNull(result.TermFrequency);
        }

        [Fact]
        public void Parse_Should_Return_Single_Keyword()
        {
            var result = _parser.Parse("apple");

            Assert.Single(result.Terms);
            Assert.Equal("apple", result.Terms[0]);
            Assert.Equal("apple", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["apple"]);
        }

        [Fact]
        public void Parse_Should_Split_By_Spaces()
        {
            var result = _parser.Parse("apple banana orange");

            Assert.Equal(3, result.Terms.Count);
            Assert.Equal(new[] { "apple", "banana", "orange" }, result.Terms);
            Assert.Equal("apple banana orange", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["apple"]);
            Assert.Equal(1, result.TermFrequency["banana"]);
            Assert.Equal(1, result.TermFrequency["orange"]);
        }

        [Fact]
        public void Parse_Should_Normalize_To_Lowercase()
        {
            var result = _parser.Parse("ApPle");

            Assert.Equal("apple", result.Terms[0]);
            Assert.Equal("ApPle", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Remove_StopWords()
        {
            var result = _parser.Parse("the apple and banana");

            Assert.Equal(new[] { "apple", "banana" }, result.Terms);
            Assert.Equal("the apple and banana", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["apple"]);
            Assert.Equal(1, result.TermFrequency["banana"]);
        }

        [Fact]
        public void Parse_Should_Ignore_Extra_Whitespace()
        {
            var result = _parser.Parse("   apple    banana   ");

            Assert.Equal(new[] { "apple", "banana" }, result.Terms);
            Assert.Equal("   apple    banana   ", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Return_Empty_List_For_Empty_String()
        {
            var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(""));
            Assert.Contains("Query cannot be null, empty, or whitespace only", exception.Message);
        }

        [Fact]
        public void Parse_Should_Return_Empty_List_For_Whitespace_Only()
        {
            var exception = Assert.Throws<ArgumentException>(() => _parser.Parse("   "));
            Assert.Contains("Query cannot be null, empty, or whitespace only", exception.Message);
        }

        [Fact]
        public void Parse_Should_Handle_Punctuation()
        {
            var result = _parser.Parse("apple, banana. orange!");

            Assert.Equal(new[] { "apple", "banana", "orange" }, result.Terms);
            Assert.Equal("apple, banana. orange!", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Handle_Control_Characters()
        {
            var result = _parser.Parse("apple\u0000banana\u0001orange");

            Assert.Equal(new[] { "apple", "banana", "orange" }, result.Terms);
            Assert.Equal("apple\u0000banana\u0001orange", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Handle_Multiple_Spaces()
        {
            var result = _parser.Parse("big   red    fox");

            Assert.Equal(new[] { "big", "red", "fox" }, result.Terms);
            Assert.Equal("big   red    fox", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Handle_Mixed_Case()
        {
            var result = _parser.Parse("HeLLo WoRLd");

            Assert.Equal(new[] { "hello", "world" }, result.Terms);
            Assert.Equal("HeLLo WoRLd", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Handle_Stopword_Removal_Example()
        {
            var result = _parser.Parse("The rain in Spain");

            Assert.Equal(new[] { "rain", "spain" }, result.Terms);
            Assert.Equal("The rain in Spain", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["rain"]);
            Assert.Equal(1, result.TermFrequency["spain"]);
        }

        [Fact]
        public void Parse_Should_Handle_Duplicate_Terms()
        {
            var result = _parser.Parse("apple apple banana");

            Assert.Equal(new[] { "apple", "apple", "banana" }, result.Terms);
            Assert.Equal("apple apple banana", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(2, result.TermFrequency["apple"]);
            Assert.Equal(1, result.TermFrequency["banana"]);
        }

        [Fact]
        public void Parse_Should_Handle_All_Stopwords()
        {
            var result = _parser.Parse("the and or but in on at to for of with by");

            Assert.Empty(result.Terms);
            Assert.Equal("the and or but in on at to for of with by", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Empty(result.TermFrequency);
        }

        [Fact]
        public void Parse_Should_Handle_Complex_Punctuation()
        {
            var result = _parser.Parse("hello@world#test$123%");

            Assert.Equal(new[] { "hello", "world", "test", "123" }, result.Terms);
            Assert.Equal("hello@world#test$123%", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Handle_Newlines_And_Tabs()
        {
            var result = _parser.Parse("apple\tbanana\norange\rgrape");

            Assert.Equal(new[] { "apple", "banana", "orange", "grape" }, result.Terms);
            Assert.Equal("apple\tbanana\norange\rgrape", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
        }

        [Fact]
        public void Parse_Should_Handle_Null_Input()
        {
            var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(null));
            Assert.Contains("Query cannot be null, empty, or whitespace only", exception.Message);
        }

        [Fact]
        public void Parse_Should_Handle_Single_Stopword()
        {
            var result = _parser.Parse("the");

            Assert.Empty(result.Terms);
            Assert.Equal("the", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Empty(result.TermFrequency);
        }

        [Fact]
        public void Parse_Should_Handle_Stopwords_With_Punctuation()
        {
            var result = _parser.Parse("the, and. or!");

            Assert.Empty(result.Terms);
            Assert.Equal("the, and. or!", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Empty(result.TermFrequency);
        }

        [Fact]
        public void Parse_Should_Handle_Mixed_Content_With_Stopwords()
        {
            var result = _parser.Parse("The quick brown fox jumps over the lazy dog");

            Assert.Equal(new[] { "quick", "brown", "fox", "jump", "lazy", "dog" }, result.Terms);
            Assert.Equal("The quick brown fox jumps over the lazy dog", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["quick"]);
            Assert.Equal(1, result.TermFrequency["brown"]);
            Assert.Equal(1, result.TermFrequency["fox"]);
            Assert.Equal(1, result.TermFrequency["jump"]);
            Assert.Equal(1, result.TermFrequency["lazy"]);
            Assert.Equal(1, result.TermFrequency["dog"]);
        }

        // Porter Stemmer Tests
        [Fact]
        public void Parse_Should_Stem_Plural_Nouns()
        {
            var result = _parser.Parse("cats dogs houses");

            Assert.Equal(new[] { "cat", "dog", "hous" }, result.Terms);
            Assert.Equal("cats dogs houses", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["cat"]);
            Assert.Equal(1, result.TermFrequency["dog"]);
            Assert.Equal(1, result.TermFrequency["hous"]);
        }

        [Fact]
        public void Parse_Should_Stem_Verb_Forms()
        {
            var result = _parser.Parse("running jumping walking");

            Assert.Equal(new[] { "run", "jump", "walk" }, result.Terms);
            Assert.Equal("running jumping walking", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["run"]);
            Assert.Equal(1, result.TermFrequency["jump"]);
            Assert.Equal(1, result.TermFrequency["walk"]);
        }

        [Fact]
        public void Parse_Should_Stem_Adjective_Forms()
        {
            var result = _parser.Parse("quickly slowly happily");

            Assert.Equal(new[] { "quick", "slow", "happi" }, result.Terms);
            Assert.Equal("quickly slowly happily", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["quick"]);
            Assert.Equal(1, result.TermFrequency["slow"]);
            Assert.Equal(1, result.TermFrequency["happi"]);
        }

        [Fact]
        public void Parse_Should_Stem_Complex_Word_Forms()
        {
            var result = _parser.Parse("nationalization internationalization");

            Assert.Equal(new[] { "nation", "internation" }, result.Terms);
            Assert.Equal("nationalization internationalization", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["nation"]);
            Assert.Equal(1, result.TermFrequency["internation"]);
        }

        [Fact]
        public void Parse_Should_Stem_Ing_Endings()
        {
            var result = _parser.Parse("fishing swimming reading");

            Assert.Equal(new[] { "fish", "swim", "read" }, result.Terms);
            Assert.Equal("fishing swimming reading", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["fish"]);
            Assert.Equal(1, result.TermFrequency["swim"]);
            Assert.Equal(1, result.TermFrequency["read"]);
        }

        [Fact]
        public void Parse_Should_Stem_Ed_Endings()
        {
            var result = _parser.Parse("walked talked played");

            Assert.Equal(new[] { "walk", "talk", "playe" }, result.Terms);
            Assert.Equal("walked talked played", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["walk"]);
            Assert.Equal(1, result.TermFrequency["talk"]);
            Assert.Equal(1, result.TermFrequency["playe"]);
        }

        [Fact]
        public void Parse_Should_Stem_Er_Endings()
        {
            var result = _parser.Parse("faster stronger better");

            Assert.Equal(new[] { "fast", "strong", "bett" }, result.Terms);
            Assert.Equal("faster stronger better", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["fast"]);
            Assert.Equal(1, result.TermFrequency["strong"]);
            Assert.Equal(1, result.TermFrequency["bett"]);
        }

        [Fact]
        public void Parse_Should_Stem_Est_Endings()
        {
            var result = _parser.Parse("fastest strongest best");

            Assert.Equal(new[] { "fast", "strong", "best" }, result.Terms);
            Assert.Equal("fastest strongest best", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["fast"]);
            Assert.Equal(1, result.TermFrequency["strong"]);
            Assert.Equal(1, result.TermFrequency["best"]);
        }

        [Fact]
        public void Parse_Should_Stem_Ly_Endings()
        {
            var result = _parser.Parse("quickly slowly carefully");

            Assert.Equal(new[] { "quick", "slow", "care" }, result.Terms);
            Assert.Equal("quickly slowly carefully", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["quick"]);
            Assert.Equal(1, result.TermFrequency["slow"]);
            Assert.Equal(1, result.TermFrequency["care"]);
        }

        [Fact]
        public void Parse_Should_Stem_Al_Endings()
        {
            var result = _parser.Parse("national personal original");

            Assert.Equal(new[] { "nation", "person", "origin" }, result.Terms);
            Assert.Equal("national personal original", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["nation"]);
            Assert.Equal(1, result.TermFrequency["person"]);
            Assert.Equal(1, result.TermFrequency["origin"]);
        }

        [Fact]
        public void Parse_Should_Stem_Complex_Word_Forms_With_Different_Patterns()
        {
            var result = _parser.Parse("nationalization internationalization");

            Assert.Equal(new[] { "nation", "internation" }, result.Terms);
            Assert.Equal("nationalization internationalization", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["nation"]);
            Assert.Equal(1, result.TermFrequency["internation"]);
        }

        [Fact]
        public void Parse_Should_Stem_Mixed_Word_Forms()
        {
            var result = _parser.Parse("running cats quickly national");

            Assert.Equal(new[] { "run", "cat", "quick", "nation" }, result.Terms);
            Assert.Equal("running cats quickly national", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["run"]);
            Assert.Equal(1, result.TermFrequency["cat"]);
            Assert.Equal(1, result.TermFrequency["quick"]);
            Assert.Equal(1, result.TermFrequency["nation"]);
        }

        [Fact]
        public void Parse_Should_Stem_With_Stopwords()
        {
            var result = _parser.Parse("the running cats and quickly jumping dogs");

            Assert.Equal(new[] { "run", "cat", "quick", "jump", "dog" }, result.Terms);
            Assert.Equal("the running cats and quickly jumping dogs", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["run"]);
            Assert.Equal(1, result.TermFrequency["cat"]);
            Assert.Equal(1, result.TermFrequency["quick"]);
            Assert.Equal(1, result.TermFrequency["jump"]);
            Assert.Equal(1, result.TermFrequency["dog"]);
        }

        [Fact]
        public void Parse_Should_Stem_Short_Words_Appropriately()
        {
            var result = _parser.Parse("at cat the dog");

            Assert.Equal(new[] { "cat", "dog" }, result.Terms);
            Assert.Equal("at cat the dog", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["cat"]);
            Assert.Equal(1, result.TermFrequency["dog"]);
        }

        [Fact]
        public void Parse_Should_Stem_With_Punctuation()
        {
            var result = _parser.Parse("running, cats! quickly; national.");

            Assert.Equal(new[] { "run", "cat", "quick", "nation" }, result.Terms);
            Assert.Equal("running, cats! quickly; national.", result.OriginalQuery);
            Assert.True(result.HasStopwordsRemoved);
            Assert.Equal(1, result.TermFrequency["run"]);
            Assert.Equal(1, result.TermFrequency["cat"]);
            Assert.Equal(1, result.TermFrequency["quick"]);
            Assert.Equal(1, result.TermFrequency["nation"]);
        }

        [Fact]
        public void Parse_Should_Respect_Configuration_Options()
        {
            var configuration = new QueryParserConfiguration
            {
                EnableStemming = false,
                RemoveStopwords = false,
                ConvertToLowercase = false,
                MinimumWordLength = 3,
                MaximumWordLength = 10
            };
            
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            var parser = new QueryParser(stemmingService, stopwordsProvider, configuration);
            
            var result = parser.Parse("The Quick Brown Fox");
            
            Assert.Equal("The Quick Brown Fox", result.OriginalQuery);
            Assert.False(result.HasStopwordsRemoved);
            Assert.Contains("The", result.Terms);
            Assert.Contains("Quick", result.Terms);
            Assert.Contains("Brown", result.Terms);
            Assert.Contains("Fox", result.Terms);
        }
        
        [Fact]
        public void Parse_Should_Respect_Minimum_Word_Length()
        {
            var configuration = new QueryParserConfiguration
            {
                MinimumWordLength = 4,
                MaximumWordLength = 20,
                RemoveStopwords = false // Don't remove stopwords for this test
            };
            
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            var parser = new QueryParser(stemmingService, stopwordsProvider, configuration);
            
            var result = parser.Parse("a an the cat dog bird");
            
            // Should only include words with length >= 4
            Assert.Contains("bird", result.Terms);
            Assert.DoesNotContain("a", result.Terms);
            Assert.DoesNotContain("an", result.Terms);
            Assert.DoesNotContain("the", result.Terms);
            Assert.DoesNotContain("cat", result.Terms);
            Assert.DoesNotContain("dog", result.Terms);
        }
        
        [Fact]
        public void Parse_Should_Respect_Maximum_Word_Length()
        {
            var configuration = new QueryParserConfiguration
            {
                MinimumWordLength = 1,
                MaximumWordLength = 5,
                RemoveStopwords = false // Don't remove stopwords for this test
            };
            
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            var parser = new QueryParser(stemmingService, stopwordsProvider, configuration);
            
            var result = parser.Parse("a cat dog bird elephant");
            
            // Should only include words with length <= 5
            Assert.Contains("a", result.Terms);
            Assert.Contains("cat", result.Terms);
            Assert.Contains("dog", result.Terms);
            Assert.Contains("bird", result.Terms);
            Assert.DoesNotContain("elephant", result.Terms);
        }
        
        [Fact]
        public void Parse_Should_Use_Custom_Tokenization_Pattern()
        {
            var configuration = new QueryParserConfiguration
            {
                TokenizationPattern = @"\s+", // Split only on whitespace
                RemoveStopwords = false
            };
            
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            var parser = new QueryParser(stemmingService, stopwordsProvider, configuration);
            
            var result = parser.Parse("cat,dog;bird:fox");
            
            // Should treat punctuation as part of words since we're only splitting on whitespace
            Assert.Contains("cat,dog;bird:fox", result.Terms);
        }
        
        [Fact]
        public void Parse_Should_Handle_Empty_Query_With_Exception()
        {
            var parser = QueryParserFactory.CreateDefault();
            
            var exception = Assert.Throws<ArgumentException>(() => parser.Parse(""));
            Assert.Contains("Query cannot be null, empty, or whitespace only", exception.Message);
        }
        
        [Fact]
        public void Parse_Should_Handle_Whitespace_Only_Query_With_Exception()
        {
            var parser = QueryParserFactory.CreateDefault();
            
            var exception = Assert.Throws<ArgumentException>(() => parser.Parse("   "));
            Assert.Contains("Query cannot be null, empty, or whitespace only", exception.Message);
        }
        
        [Fact]
        public void Parse_Should_Handle_Null_Query_With_Exception()
        {
            var parser = QueryParserFactory.CreateDefault();
            
            var exception = Assert.Throws<ArgumentException>(() => parser.Parse(null));
            Assert.Contains("Query cannot be null, empty, or whitespace only", exception.Message);
        }
        
        [Fact]
        public void Parse_Should_Use_Stopwords_Provider()
        {
            var configuration = QueryParserConfiguration.CreateDefault();
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            var parser = new QueryParser(stemmingService, stopwordsProvider, configuration);
            
            var result = parser.Parse("the quick brown fox");
            
            // Should remove stopwords using the provider
            Assert.DoesNotContain("the", result.Terms);
            Assert.Contains("quick", result.Terms);
            Assert.Contains("brown", result.Terms);
            Assert.Contains("fox", result.Terms);
        }
        
        [Fact]
        public void Parse_Should_Handle_Configuration_Validation()
        {
            var configuration = new QueryParserConfiguration
            {
                MinimumWordLength = 5,
                MaximumWordLength = 3 // Invalid: max < min
            };
            
            var stopwordsProvider = new DefaultStopwordsProvider();
            var stemmingService = new StemmingService(new PorterStemmer());
            
            // Should throw during construction due to invalid configuration
            var exception = Assert.Throws<ArgumentException>(() => 
                new QueryParser(stemmingService, stopwordsProvider, configuration));
            Assert.Contains("MaximumWordLength", exception.Message);
        }
    }
}