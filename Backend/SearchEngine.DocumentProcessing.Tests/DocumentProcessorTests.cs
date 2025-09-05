using SearchEngine.DocumentProcessing.Services;
using SearchEngine.DocumentProcessing.Interfaces;
using SearchEngine.DocumentProcessing.Parsers;

namespace SearchEngine.DocumentProcessing.Tests
{
    public class DocumentProcessorTests
    {
        [Fact]
        public void Constructor_Should_Initialize_With_Parser()
        {
            // Arrange
            var mockParser = new MockParser();

            // Act
            var processor = new DocumentProcessor(mockParser);

            // Assert
            Assert.NotNull(processor);
        }

        [Fact]
        public void Constructor_Should_Accept_Null_Parser()
        {
            // Act - The current implementation doesn't validate null, so it should not throw
            var processor = new DocumentProcessor(null!);
            
            // Assert
            Assert.NotNull(processor);
        }

        [Fact]
        public void ProcessDocument_Should_Return_Tokens_For_Valid_File()
        {
            // Arrange
            var mockParser = new MockParser();
            var processor = new DocumentProcessor(mockParser);
            var testFilePath = "test.txt";

            // Act
            var result = processor.processDocument(testFilePath);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Tokenizer.Token>>(result);
        }

        [Fact]
        public void ProcessDocument_Should_Call_Parser_ReadContent()
        {
            // Arrange
            var mockParser = new MockParser();
            var processor = new DocumentProcessor(mockParser);
            var testFilePath = "test.txt";

            // Act
            processor.processDocument(testFilePath);

            // Assert
            Assert.True(mockParser.ReadContentCalled);
            Assert.Equal(testFilePath, mockParser.LastFilePath);
        }

        [Fact]
        public void ProcessDocument_Should_Handle_Empty_Text()
        {
            // Arrange
            var mockParser = new MockParser { ReturnText = "" };
            var processor = new DocumentProcessor(mockParser);

            // Act
            var result = processor.processDocument("test.txt");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ProcessDocument_Should_Handle_Whitespace_Only_Text()
        {
            // Arrange
            var mockParser = new MockParser { ReturnText = "   \n\t   " };
            var processor = new DocumentProcessor(mockParser);

            // Act
            var result = processor.processDocument("test.txt");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ProcessDocument_Should_Process_Simple_Text()
        {
            // Arrange
            var mockParser = new MockParser { ReturnText = "Hello world test" };
            var processor = new DocumentProcessor(mockParser);

            // Act
            var result = processor.processDocument("test.txt");

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            // Should have processed tokens (exact count depends on stopword removal)
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void ProcessDocument_Should_Remove_Stopwords()
        {
            // Arrange
            var mockParser = new MockParser { ReturnText = "the quick brown fox" };
            var processor = new DocumentProcessor(mockParser);

            // Act
            var result = processor.processDocument("test.txt");

            // Assert
            Assert.NotNull(result);
            // Should not contain stopwords like "the"
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.DoesNotContain("the", tokenStrings);
        }

        [Fact]
        public void ProcessDocument_Should_Apply_Stemming()
        {
            // Arrange
            var mockParser = new MockParser { ReturnText = "running jumping cats" };
            var processor = new DocumentProcessor(mockParser);

            // Act
            var result = processor.processDocument("test.txt");

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            // Should have stemmed words (exact values depend on Porter2Stemmer implementation)
            var tokenStrings = result.Select(t => t.String).ToList();
            Assert.Contains("run", tokenStrings); // "running" should be stemmed to "run"
        }

        [Fact]
        public void ProcessDocument_Should_Set_Correct_Positions()
        {
            // Arrange
            var mockParser = new MockParser { ReturnText = "hello world test" };
            var processor = new DocumentProcessor(mockParser);

            // Act
            var result = processor.processDocument("test.txt");

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            // Check that positions are sequential
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(i, result[i].Position);
            }
        }

        // Mock parser for testing
        private class MockParser : IParser
        {
            public string ReturnText { get; set; } = "Sample text for testing";
            public bool ReadContentCalled { get; private set; }
            public string LastFilePath { get; private set; } = string.Empty;

            public string ReadContent(string filePath)
            {
                ReadContentCalled = true;
                LastFilePath = filePath;
                return ReturnText;
            }
        }
    }
}
