using SearchEngine.DocumentProcessing.Interfaces;

namespace SearchEngine.DocumentProcessing.Services
{
  /// <summary>
  /// Processes documents by parsing, tokenizing and normalizing text.
  /// </summary>
  public class DocumentProcessor
  {
    private readonly IParser _parser;
    private readonly Tokenizer _tokenizer;
    private readonly Normalizer _normalizer;

    /// <summary>
    /// Initializes a new instance of <see cref="DocumentProcessor"/>
    /// </summary>
    /// <param name="parser">The parser used to read document content</param>
    public DocumentProcessor(IParser parser)
    {
      _parser = parser;
      _tokenizer = new Tokenizer();
      _normalizer = new Normalizer();
    }

    /// <summary>
    /// Processes a document at the given path
    /// </summary>
    /// <param name="path">The path to the document file</param>
    /// <returns>A list of normalized tokens extracted from the document </returns>
    public List<Tokenizer.Token> processDocument(string path)
    {
      var parsedText = _parser.ReadContent(path);
      var tokenizedText = _tokenizer.Tokenize(parsedText);
      var normalizedText = _normalizer.Normalize(tokenizedText);
      return normalizedText;
    }
  }
}
