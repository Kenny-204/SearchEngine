using Porter2Stemmer;

namespace SearchEngine.DocumentProcessing.Services
{
  /// <summary>
  /// Tokenizes text into individual tokens and applies stemming
  /// </summary>
  public class Tokenizer
  {
    /// <summary>
    /// Represents a token extracted from text
    /// </summary>
    public class Token
    {
      public string String { get; set; }
      public int Position { get; set; }

      public Token(string String, int Position)
      {
        this.String = String;
        this.Position = Position;
      }
    }
    /// <summary>
    /// splits text into tokens, converts to lowercase, and applies stemming
    /// </summary>
    /// <param name="text">The input text to tokenize</param>
    /// <returns>A list of <see cref="Token"/> objects representing the tokenised text</returns>

    public List<Token> Tokenize(string text)
    {
      if (string.IsNullOrWhiteSpace(text))
        return new List<Token> { };
      string lowerCaseText = text.ToLower();
      var tokens = lowerCaseText.Split(
        new[] { ' ', '\n', '\r', '\t', '?', '"', '\'', ':', ',', '(', ')', '[', ']', '-', '_', '/', '.' },
        StringSplitOptions.RemoveEmptyEntries
      );

      for (int i = 0; i < tokens.Length; i++)
      {
        var stemmer = new EnglishPorter2Stemmer();
        tokens[i] = stemmer.Stem(tokens[i]).Value;
      }
      return tokens.Select((token, index) => new Token(token, index)).ToList();
    }
  }
}
