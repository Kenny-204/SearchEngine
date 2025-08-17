using System.Collections.Generic;

namespace SearchEngine.Query.Core
{
    /// <summary>
    /// Interface for providing stopwords to the QueryParser
    /// </summary>
    public interface IStopwordsProvider
    {
        /// <summary>
        /// Gets the collection of stopwords
        /// </summary>
        /// <returns>HashSet of stopwords in lowercase</returns>
        HashSet<string> GetStopwords();
        
        /// <summary>
        /// Checks if a word is a stopword
        /// </summary>
        /// <param name="word">The word to check (should be lowercase)</param>
        /// <returns>True if the word is a stopword</returns>
        bool IsStopword(string word);
        
        /// <summary>
        /// Reloads stopwords from the source (useful for configuration changes)
        /// </summary>
        void Reload();
    }
} 