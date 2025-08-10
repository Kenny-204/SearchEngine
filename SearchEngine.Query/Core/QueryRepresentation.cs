using System.Collections.Generic;

namespace SearchEngine.Query.Core
{
    public class QueryRepresentation
    {
        public string OriginalQuery { get; set; } = string.Empty;
        public List<string> Terms { get; set; } = new List<string>();
        public bool HasStopwordsRemoved { get; set; }
        public Dictionary<string, int> TermFrequency { get; set; } = new Dictionary<string, int>();
    }
} 