using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SearchEngine.Query.Core;

namespace SearchEngine.Query.Services
{
    /// <summary>
    /// Default implementation of IStopwordsProvider that loads stopwords from embedded resources
    /// </summary>
    public class DefaultStopwordsProvider : IStopwordsProvider
    {
        private HashSet<string> _stopwords = new HashSet<string>();
        private readonly object _lockObject = new object();

        public DefaultStopwordsProvider()
        {
            LoadStopwords();
        }

        public HashSet<string> GetStopwords()
        {
            lock (_lockObject)
            {
                return new HashSet<string>(_stopwords, StringComparer.OrdinalIgnoreCase);
            }
        }

        public bool IsStopword(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;

            lock (_lockObject)
            {
                return _stopwords.Contains(word);
            }
        }

        public void Reload()
        {
            LoadStopwords();
        }

        private void LoadStopwords()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "SearchEngine.Query.Resources.stopwords.txt";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var content = reader.ReadToEnd();
                            var words = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                             .Select(w => w.Trim().ToLowerInvariant())
                                             .Where(w => !string.IsNullOrEmpty(w))
                                             .ToHashSet(StringComparer.OrdinalIgnoreCase);

                            lock (_lockObject)
                            {
                                _stopwords = words;
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Fallback to hardcoded stopwords if resource loading fails
            }

            LoadFallbackStopwords();
        }

        private void LoadFallbackStopwords()
        {
            var fallbackStopwords = new[]
            {
                "a", "an", "and", "are", "as", "at", "be", "by", "for", "from", "has", "he", "in", "is", "it", "its",
                "of", "on", "that", "the", "to", "was", "will", "with", "the", "a", "an", "and", "or", "but", "in", "on",
                "at", "to", "for", "of", "with", "by", "up", "down", "out", "off", "over", "under", "again", "further",
                "then", "once", "here", "there", "when", "where", "why", "how", "all", "any", "both", "each", "few",
                "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own", "same", "so", "than",
                "too", "very", "can", "will", "just", "should", "now", "i", "me", "my", "myself", "we", "our", "ours",
                "ourselves", "you", "your", "yours", "yourself", "yourselves", "he", "him", "his", "himself", "she",
                "her", "hers", "herself", "it", "its", "itself", "they", "them", "their", "theirs", "themselves",
                "what", "which", "who", "whom", "this", "that", "these", "those", "am", "is", "are", "was", "were",
                "been", "being", "have", "has", "had", "having", "do", "does", "did", "doing", "would", "should",
                "could", "ought", "i'm", "you're", "he's", "she's", "it's", "we're", "they're", "i've", "you've",
                "we've", "they've", "i'd", "you'd", "he'd", "she'd", "we'd", "they'd", "i'll", "you'll", "he'll",
                "she'll", "we'll", "they'll", "isn't", "aren't", "wasn't", "weren't", "hasn't", "haven't", "hadn't",
                "doesn't", "don't", "didn't", "won't", "wouldn't", "shan't", "shouldn't", "can't", "cannot", "couldn't",
                "mustn't", "let's", "that's", "who's", "what's", "here's", "there's", "when's", "where's", "why's",
                "how's", "a", "an", "the", "and", "or", "but", "if", "then", "else", "when", "at", "from", "into",
                "during", "including", "until", "against", "among", "throughout", "despite", "towards", "upon", "concerning",
                "to", "in", "for", "of", "with", "by", "about", "against", "between", "into", "through", "during",
                "before", "after", "above", "below", "from", "up", "down", "out", "off", "over", "under", "again",
                "further", "then", "once", "here", "there", "when", "where", "why", "how", "all", "any", "both",
                "each", "few", "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own", "same",
                "so", "than", "too", "very", "can", "will", "just", "should", "now"
            };

            lock (_lockObject)
            {
                _stopwords = new HashSet<string>(fallbackStopwords, StringComparer.OrdinalIgnoreCase);
            }
        }
    }
} 