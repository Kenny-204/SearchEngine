namespace SearchEngine.Query
{
    /// <summary>
    /// Interface for Porter Stemmer operations
    /// </summary>
    public interface IPorterStemmer
    {
        /// <summary>
        /// Stems a single word using the Porter Stemmer algorithm
        /// </summary>
        /// <param name="word">The word to stem</param>
        /// <returns>The stemmed form of the word</returns>
        string Stem(string word);
        
        /// <summary>
        /// Stems multiple words using the Porter Stemmer algorithm
        /// </summary>
        /// <param name="words">Collection of words to stem</param>
        /// <returns>Collection of stemmed words</returns>
        IEnumerable<string> Stem(IEnumerable<string> words);
    }
} 