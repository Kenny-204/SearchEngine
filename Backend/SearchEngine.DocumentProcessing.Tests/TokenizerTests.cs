using SearchEngine.DocumentProcessing.Services;

namespace SearchEngine.DocumentProcessing.Tests
{
    public class TokenizerTests
    {
        private readonly Tokenizer _tokenizer;

        public TokenizerTests()
        {
            _tokenizer = new Tokenizer();
        }

        [Fact]
        public void Tokenize_Should_Return_Empty_List_For_Null_Input()
        {
            // Act
            var result = _tokenizer.Tokenize(null!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Tokenize_Should_Return_Empty_List_For_Empty_String()
        {
            // Act
            var result = _tokenizer.Tokenize("");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Tokenize_Should_Return_Empty_List_For_Whitespace_Only()
        {
            // Act
            var result = _tokenizer.Tokenize("   \n\t   ");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Tokenize_Should_Split_On_Multiple_Delimiters()
        {
            // Arrange
            var text = "hello,world!test?text:here";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void Tokenize_Should_Convert_To_Lowercase()
        {
            // Arrange
            var text = "Hello WORLD Test";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            foreach (var token in result)
            {
                Assert.Equal(token.String.ToLower(), token.String);
            }
        }

        [Fact]
        public void Tokenize_Should_Apply_Stemming()
        {
            // Arrange
            var text = "running jumping cats";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
            
            // Check that stemming was applied (exact values depend on Porter2Stemmer)
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.Contains("run", tokenStrings); // "running" -> "run"
            Assert.Contains("jump", tokenStrings); // "jumping" -> "jump"
            Assert.Contains("cat", tokenStrings); // "cats" -> "cat"
        }

        [Fact]
        public void Tokenize_Should_Set_Correct_Positions()
        {
            // Arrange
            var text = "hello world test";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            
            Assert.Equal(0, result[0].Position);
            Assert.Equal(1, result[1].Position);
            Assert.Equal(2, result[2].Position);
        }

        [Fact]
        public void Tokenize_Should_Handle_Complex_Punctuation()
        {
            // Arrange
            var text = "hello, world! (test) [example] - another_word/here.txt";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            // Should split on all punctuation marks
            Assert.True(result.Count > 4);
        }

        [Fact]
        public void Tokenize_Should_Handle_Multiple_Spaces()
        {
            // Arrange
            var text = "hello    world     test";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Tokenize_Should_Handle_Newlines_And_Tabs()
        {
            // Arrange
            var text = "hello\nworld\ttest";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Tokenize_Should_Handle_Mixed_Case_And_Punctuation()
        {
            // Arrange
            var text = "Hello, World! How are you?";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            // All tokens should be lowercase
            foreach (var token in result)
            {
                Assert.Equal(token.String.ToLower(), token.String);
            }
        }

        [Fact]
        public void Tokenize_Should_Handle_Single_Word()
        {
            // Arrange
            var text = "hello";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("hello", result[0].String);
            Assert.Equal(0, result[0].Position);
        }

        [Fact]
        public void Tokenize_Should_Handle_Numbers()
        {
            // Arrange
            var text = "123 test 456";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains("123", result.Select(t => t.String));
            Assert.Contains("456", result.Select(t => t.String));
        }

        [Fact]
        public void Tokenize_Should_Handle_Special_Characters()
        {
            // Arrange
            var text = "test@email.com user_name #hashtag $money";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            // Should split on special characters
            Assert.True(result.Count > 4);
        }

        [Fact]
        public void Tokenize_Token_Properties_Should_Be_Set_Correctly()
        {
            // Arrange
            var text = "hello world";

            // Act
            var result = _tokenizer.Tokenize(text);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            
            var firstToken = result[0];
            Assert.Equal("hello", firstToken.String);
            Assert.Equal(0, firstToken.Position);
            
            var secondToken = result[1];
            Assert.Equal("world", secondToken.String);
            Assert.Equal(1, secondToken.Position);
        }
    }
}
