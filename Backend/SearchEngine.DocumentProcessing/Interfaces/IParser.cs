namespace SearchEngine.DocumentProcessing.Interfaces
{
  /// <summary>
  /// The interface all parsers would implement
  /// </summary>
  public interface IParser
  {
    public string ReadContent(string filePath);
  }
}
