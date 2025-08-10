using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Services
{
    public class DocumentProcessor
    {
        private readonly IParser _parser;
        private readonly Tokenizer _tokenizer;
        private readonly Normalizer _normalizer;

        public DocumentProcessor(IParser parser)
        {
            _parser = parser;
            _tokenizer = new Tokenizer();
            _normalizer = new Normalizer();
        }

        public List<Tokenizer.Token> processDocument(string text)
        {
            var parsedText = _parser.ReadContent(text);
            var tokenizedText = _tokenizer.Tokenize(parsedText);
            var normalizedText = _normalizer.Normalize(tokenizedText);
            return normalizedText;
        }
    }
}
