using SearchEngine.DocumentProcessing.Services;

namespace SearchEngine.DocumentProcessing.Tests
{
    public class NormalizerTests
    {
        private readonly Normalizer _normalizer;

        public NormalizerTests()
        {
            _normalizer = new Normalizer();
        }

        [Fact]
        public void Normalize_Should_Return_Empty_List_For_Empty_Input()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>();

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Normalize_Should_Throw_For_Null_Input()
        {
            // Act & Assert - The current implementation throws ArgumentNullException for null input
            Assert.Throws<ArgumentNullException>(() => _normalizer.Normalize(null!));
        }

        [Fact]
        public void Normalize_Should_Remove_Common_Stopwords()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("the", 0),
                new Tokenizer.Token("quick", 1),
                new Tokenizer.Token("brown", 2),
                new Tokenizer.Token("fox", 3),
                new Tokenizer.Token("and", 4),
                new Tokenizer.Token("the", 5),
                new Tokenizer.Token("lazy", 6),
                new Tokenizer.Token("dog", 7)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count); // Should remove "the", "and", "the" but keep "lazy" and "dog"
            
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.Contains("quick", tokenStrings);
            Assert.Contains("brown", tokenStrings);
            Assert.Contains("fox", tokenStrings);
            Assert.Contains("lazy", tokenStrings);
            Assert.Contains("dog", tokenStrings);
            Assert.DoesNotContain("the", tokenStrings);
            Assert.DoesNotContain("and", tokenStrings);
        }

        [Fact]
        public void Normalize_Should_Preserve_Non_Stopwords()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("computer", 0),
                new Tokenizer.Token("algorithm", 1),
                new Tokenizer.Token("search", 2),
                new Tokenizer.Token("engine", 3)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.Contains("computer", tokenStrings);
            Assert.Contains("algorithm", tokenStrings);
            Assert.Contains("search", tokenStrings);
            Assert.Contains("engine", tokenStrings);
        }

        [Fact]
        public void Normalize_Should_Remove_All_Stopwords()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("the", 0),
                new Tokenizer.Token("a", 1),
                new Tokenizer.Token("an", 2),
                new Tokenizer.Token("and", 3),
                new Tokenizer.Token("or", 4),
                new Tokenizer.Token("but", 5),
                new Tokenizer.Token("in", 6),
                new Tokenizer.Token("on", 7),
                new Tokenizer.Token("at", 8),
                new Tokenizer.Token("to", 9),
                new Tokenizer.Token("for", 10),
                new Tokenizer.Token("of", 11),
                new Tokenizer.Token("with", 12),
                new Tokenizer.Token("by", 13)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // All tokens are stopwords
        }

        [Fact]
        public void Normalize_Should_Handle_Mixed_Case_Stopwords()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("THE", 0),
                new Tokenizer.Token("quick", 1),
                new Tokenizer.Token("AND", 2),
                new Tokenizer.Token("brown", 3)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count); // All tokens are kept since stopwords are case-sensitive
            
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.Contains("THE", tokenStrings);
            Assert.Contains("quick", tokenStrings);
            Assert.Contains("AND", tokenStrings);
            Assert.Contains("brown", tokenStrings);
        }

        [Fact]
        public void Normalize_Should_Preserve_Token_Positions()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("the", 0),
                new Tokenizer.Token("quick", 1),
                new Tokenizer.Token("brown", 2),
                new Tokenizer.Token("fox", 3)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            
            // Check that positions are preserved
            Assert.Equal(1, result[0].Position); // "quick"
            Assert.Equal(2, result[1].Position); // "brown"
            Assert.Equal(3, result[2].Position); // "fox"
        }

        [Fact]
        public void Normalize_Should_Handle_Duplicate_Stopwords()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("the", 0),
                new Tokenizer.Token("the", 1),
                new Tokenizer.Token("the", 2),
                new Tokenizer.Token("test", 3)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("test", result[0].String);
            Assert.Equal(3, result[0].Position);
        }

        [Fact]
        public void Normalize_Should_Handle_Empty_Tokens()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("", 0),
                new Tokenizer.Token("test", 1),
                new Tokenizer.Token("", 2)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count); // Current implementation keeps empty tokens
            Assert.Equal("", result[0].String);
            Assert.Equal("test", result[1].String);
            Assert.Equal("", result[2].String);
        }

        [Fact]
        public void Normalize_Should_Handle_Whitespace_Tokens()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("   ", 0),
                new Tokenizer.Token("test", 1),
                new Tokenizer.Token("\t", 2)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count); // Current implementation keeps whitespace tokens
            Assert.Equal("   ", result[0].String);
            Assert.Equal("test", result[1].String);
            Assert.Equal("\t", result[2].String);
        }

        [Fact]
        public void Normalize_Should_Handle_Large_Stopword_List()
        {
            // Arrange
            var stopwords = new[] { "a", "an", "and", "are", "as", "at", "be", "by", "for", "from", "has", "he", "in", "is", "it", "its", "of", "on", "that", "the", "to", "was", "were", "will", "with", "this", "i", "you", "your", "yours", "they", "them", "their", "what", "which", "who", "whom", "do", "does", "did", "but", "if", "or", "because", "about", "against", "between", "into", "through", "during", "before", "after", "above", "below", "up", "down", "out", "off", "over", "under", "again", "further", "then", "once", "here", "there", "when", "where", "why", "how", "all", "any", "both", "each", "few", "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own", "same", "so", "than", "too", "very", "can", "will", "just", "should", "now" };
            
            var tokens = stopwords.Select((word, index) => new Tokenizer.Token(word, index)).ToList();
            tokens.Add(new Tokenizer.Token("important", stopwords.Length));

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("important", result[0].String);
        }

        [Fact]
        public void Normalize_Should_Handle_Special_Characters_In_Tokens()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("the", 0),
                new Tokenizer.Token("test-word", 1),
                new Tokenizer.Token("and", 2),
                new Tokenizer.Token("another_word", 3)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.Contains("test-word", tokenStrings);
            Assert.Contains("another_word", tokenStrings);
        }

        [Fact]
        public void Normalize_Should_Handle_Numbers()
        {
            // Arrange
            var tokens = new List<Tokenizer.Token>
            {
                new Tokenizer.Token("the", 0),
                new Tokenizer.Token("123", 1),
                new Tokenizer.Token("and", 2),
                new Tokenizer.Token("456", 3)
            };

            // Act
            var result = _normalizer.Normalize(tokens);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.Contains("123", tokenStrings);
            Assert.Contains("456", tokenStrings);
        }
    }
}
