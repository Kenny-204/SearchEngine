using SearchEngine.DocumentProcessing.Interfaces;
using SearchEngine.DocumentProcessing.Parsers;

namespace SearchEngine.DocumentProcessing.Services
{
    public class ParserFactory
    {
        public static IParser GetParser(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".txt":
                    return new PlainTextParser();
                default:
                    throw new Exception($"File type {extension} not supported");
            }
        }
    }
}
