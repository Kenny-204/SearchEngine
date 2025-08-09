namespace SearchEngine.DocumentProcessing.Services
{
    public class Normalizer
    {
        public List<string> Normalize(List<string> tokenList)
        {
            var stopWord = new HashSet<string> {
                "a", "an", "and", "are", "as", "at", "be", "by",
                "for", "from", "has", "he", "in", "is", "it",
                "its", "of", "on", "that", "the", "to", "was",
                "were", "will", "with", "this", "i", "you",
                "your", "yours", "they", "them", "their", "what",
                "which", "who", "whom", "do", "does", "did",
                "but", "if", "or", "because", "about", "against",
                "between", "into", "through", "during", "before",
                "after", "above", "below", "up", "down", "out",
                "off", "over", "under", "again", "further", "then",
                "once", "here", "there", "when", "where", "why",
                "how", "all", "any", "both", "each", "few", "more",
                "most", "other", "some", "such", "no", "nor", "not",
                "only", "own", "same", "so", "than", "too", "very",
                "can", "will", "just", "should", "now"
            };

            var normalizedTokens = tokenList.Where(token => !stopWord.Contains(token)).ToList();

            return normalizedTokens;

        }
    }
}
