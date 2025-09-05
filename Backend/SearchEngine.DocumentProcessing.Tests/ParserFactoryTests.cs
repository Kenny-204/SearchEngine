using SearchEngine.DocumentProcessing.Services;
using SearchEngine.DocumentProcessing.Parsers;

namespace SearchEngine.DocumentProcessing.Tests
{
    public class ParserFactoryTests
    {
        [Theory]
        [InlineData("test.txt", typeof(PlainTextParser))]
        [InlineData("document.TXT", typeof(PlainTextParser))]
        [InlineData("file.Txt", typeof(PlainTextParser))]
        public void GetParser_Should_Return_PlainTextParser_For_Txt_Files(string filePath, Type expectedType)
        {
            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType(expectedType, parser);
        }

        [Theory]
        [InlineData("test.html", typeof(HTMLParser))]
        [InlineData("document.HTML", typeof(HTMLParser))]
        [InlineData("file.Html", typeof(HTMLParser))]
        public void GetParser_Should_Return_HTMLParser_For_Html_Files(string filePath, Type expectedType)
        {
            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType(expectedType, parser);
        }

        [Theory]
        [InlineData("test.xml", typeof(XMLParser))]
        [InlineData("document.XML", typeof(XMLParser))]
        [InlineData("file.Xml", typeof(XMLParser))]
        public void GetParser_Should_Return_XMLParser_For_Xml_Files(string filePath, Type expectedType)
        {
            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType(expectedType, parser);
        }

        [Theory]
        [InlineData("test.pdf", typeof(PDFParser))]
        [InlineData("document.PDF", typeof(PDFParser))]
        [InlineData("file.Pdf", typeof(PDFParser))]
        public void GetParser_Should_Return_PDFParser_For_Pdf_Files(string filePath, Type expectedType)
        {
            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType(expectedType, parser);
        }

        [Theory]
        [InlineData("test.docx", typeof(DocxParser))]
        [InlineData("document.DOCX", typeof(DocxParser))]
        [InlineData("file.Docx", typeof(DocxParser))]
        public void GetParser_Should_Return_DocxParser_For_Docx_Files(string filePath, Type expectedType)
        {
            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType(expectedType, parser);
        }

        [Theory]
        [InlineData("test.pptx", typeof(PptxParser))]
        [InlineData("document.PPTX", typeof(PptxParser))]
        [InlineData("file.Pptx", typeof(PptxParser))]
        public void GetParser_Should_Return_PptxParser_For_Pptx_Files(string filePath, Type expectedType)
        {
            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType(expectedType, parser);
        }

        [Theory]
        [InlineData("test.xlsx", typeof(Xlsxparser))]
        [InlineData("document.XLSX", typeof(Xlsxparser))]
        [InlineData("file.Xlsx", typeof(Xlsxparser))]
        public void GetParser_Should_Return_Xlsxparser_For_Xlsx_Files(string filePath, Type expectedType)
        {
            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType(expectedType, parser);
        }

        [Theory]
        [InlineData("test.unknown")]
        [InlineData("document.xyz")]
        [InlineData("file.abc")]
        [InlineData("test")]
        [InlineData("document")]
        public void GetParser_Should_Throw_Exception_For_Unsupported_Files(string filePath)
        {
            // Act & Assert
            var exception = Assert.Throws<Exception>(() => ParserFactory.GetParser(filePath));
            Assert.Contains("not supported", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void GetParser_Should_Throw_Exception_For_Empty_Paths(string filePath)
        {
            // Act & Assert - Empty paths result in empty extension, which throws Exception
            Assert.Throws<Exception>(() => ParserFactory.GetParser(filePath));
        }

        [Fact]
        public void GetParser_Should_Throw_NullReferenceException_For_Null_Path()
        {
            // Act & Assert - Null path causes NullReferenceException in Path.GetExtension
            Assert.Throws<NullReferenceException>(() => ParserFactory.GetParser(null!));
        }

        [Fact]
        public void GetParser_Should_Handle_Paths_With_Directories()
        {
            // Arrange
            var filePath = @"C:\Documents\test.txt";

            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType<PlainTextParser>(parser);
        }

        [Fact]
        public void GetParser_Should_Handle_Paths_With_Multiple_Dots()
        {
            // Arrange
            var filePath = "test.backup.txt";

            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType<PlainTextParser>(parser);
        }

        [Fact]
        public void GetParser_Should_Handle_Paths_With_Spaces()
        {
            // Arrange
            var filePath = "my document.txt";

            // Act
            var parser = ParserFactory.GetParser(filePath);

            // Assert
            Assert.NotNull(parser);
            Assert.IsType<PlainTextParser>(parser);
        }

        [Fact]
        public void GetParser_Should_Return_Different_Instances()
        {
            // Arrange
            var filePath1 = "test1.txt";
            var filePath2 = "test2.txt";

            // Act
            var parser1 = ParserFactory.GetParser(filePath1);
            var parser2 = ParserFactory.GetParser(filePath2);

            // Assert
            Assert.NotNull(parser1);
            Assert.NotNull(parser2);
            Assert.IsType<PlainTextParser>(parser1);
            Assert.IsType<PlainTextParser>(parser2);
            // They should be different instances
            Assert.NotSame(parser1, parser2);
        }

        [Fact]
        public void GetParser_Should_Handle_All_Supported_Extensions()
        {
            // Arrange
            var supportedExtensions = new[]
            {
                ".txt", ".html", ".xml", ".pdf", ".docx", ".pptx", ".xlsx"
            };

            // Act & Assert
            foreach (var extension in supportedExtensions)
            {
                var filePath = $"test{extension}";
                var parser = ParserFactory.GetParser(filePath);
                Assert.NotNull(parser);
            }
        }

        [Fact]
        public void GetParser_Should_Handle_Case_Insensitive_Extensions()
        {
            // Arrange
            var testCases = new[]
            {
                ("test.TXT", typeof(PlainTextParser)),
                ("test.HTML", typeof(HTMLParser)),
                ("test.XML", typeof(XMLParser)),
                ("test.PDF", typeof(PDFParser)),
                ("test.DOCX", typeof(DocxParser)),
                ("test.PPTX", typeof(PptxParser)),
                ("test.XLSX", typeof(Xlsxparser))
            };

            // Act & Assert
            foreach (var (filePath, expectedType) in testCases)
            {
                var parser = ParserFactory.GetParser(filePath);
                Assert.NotNull(parser);
                Assert.IsType(expectedType, parser);
            }
        }
    }
}
