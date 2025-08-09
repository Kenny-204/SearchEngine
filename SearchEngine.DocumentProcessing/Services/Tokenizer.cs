namespace SearchEngine.DocumentProcessing.Services
{
    public class Tokenizer
    {
        public List<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string> { };
            string lowerCaseText = text.ToLower();
            var tokens = lowerCaseText.Split(new[] { ' ', '\n', '\r', '\t', '?', '"', '\'', '(', ')', '[', ']', '-', '_', '/', }, StringSplitOptions.RemoveEmptyEntries);
            return tokens.ToList();
        }
    }
}