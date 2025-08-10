namespace SearchEngine.DocumentProcessing.Services
{
  public class Tokenizer
  {
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

    public List<Token> Tokenize(string text)
    {
      if (string.IsNullOrWhiteSpace(text))
        return new List<Token> { };
      string lowerCaseText = text.ToLower();
      var tokens = lowerCaseText.Split(
        new[] { ' ', '\n', '\r', '\t', '?', '"', '\'', '(', ')', '[', ']', '-', '_', '/' },
        StringSplitOptions.RemoveEmptyEntries
      );
      return tokens.Select((token, index) => new Token(token, index)).ToList();
    }
  }
}
