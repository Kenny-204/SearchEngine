using System;
using System.Collections.Generic;
using System.Linq;
using SearchEngine.Query.Core;

// TF-IDF Integration Example with Query Parser Output
// This shows exactly how to use QueryRepresentation for TF-IDF calculations

namespace SearchEngine.Matching.TFIDF
{
    /// <summary>
    /// Complete TF-IDF implementation using Query Parser output
    /// </summary>
    public class TFIDFMatchingFunction
    {
        private readonly Dictionary<string, double> _idfCache;
        private readonly int _totalDocuments;
        
        public TFIDFMatchingFunction(int totalDocuments, Dictionary<string, double> idfValues)
        {
            _totalDocuments = totalDocuments;
            _idfCache = idfValues ?? new Dictionary<string, double>();
        }
        
        /// <summary>
        /// Main search method using TF-IDF with Query Parser output
        /// </summary>
        public SearchResults SearchWithTFIDF(QueryRepresentation query)
        {
            // 1. Extract data from Query Parser (this is what you need!)
            var queryTerms = query.Terms;
            var queryTermFrequencies = query.TermFrequency;
            
            // 2. Build query vector using TF-IDF
            var queryVector = BuildQueryVector(queryTerms, queryTermFrequencies);
            
            // 3. Calculate similarity with each document
            var results = new List<DocumentResult>();
            
            foreach (var document in GetAllDocuments())
            {
                var documentVector = BuildDocumentVector(document, queryTerms);
                var similarity = CalculateCosineSimilarity(queryVector, documentVector);
                
                if (similarity > 0)
                {
                    results.Add(new DocumentResult(document, similarity));
                }
            }
            
            return new SearchResults(results.OrderByDescending(r => r.Score));
        }
        
        /// <summary>
        /// Build query vector using Query Parser output
        /// </summary>
        private Dictionary<string, double> BuildQueryVector(List<string> terms, Dictionary<string, int> termFrequencies)
        {
            var vector = new Dictionary<string, double>();
            
            foreach (var term in terms)
            {
                // Get TF directly from Query Parser - no need to calculate!
                var tf = termFrequencies[term];
                
                // Get IDF for this term
                var idf = GetInverseDocumentFrequency(term);
                
                // Calculate TF-IDF: tf * idf
                var tfidf = tf * idf;
                vector[term] = tfidf;
            }
            
            return vector;
        }
        
        /// <summary>
        /// Build document vector for comparison
        /// </summary>
        private Dictionary<string, double> BuildDocumentVector(Document document, List<string> queryTerms)
        {
            var vector = new Dictionary<string, double>();
            
            foreach (var term in queryTerms)
            {
                // Calculate TF for this term in the document
                var tf = CalculateTermFrequencyInDocument(term, document);
                
                // Get IDF for this term
                var idf = GetInverseDocumentFrequency(term);
                
                // Calculate TF-IDF for document
                var tfidf = tf * idf;
                vector[term] = tfidf;
            }
            
            return vector;
        }
        
        /// <summary>
        /// Calculate cosine similarity between query and document vectors
        /// </summary>
        private double CalculateCosineSimilarity(Dictionary<string, double> queryVector, Dictionary<string, double> documentVector)
        {
            var dotProduct = 0.0;
            var queryMagnitude = 0.0;
            var documentMagnitude = 0.0;
            
            foreach (var term in queryVector.Keys)
            {
                if (documentVector.ContainsKey(term))
                {
                    dotProduct += queryVector[term] * documentVector[term];
                }
                
                queryMagnitude += queryVector[term] * queryVector[term];
            }
            
            foreach (var value in documentVector.Values)
            {
                documentMagnitude += value * value;
            }
            
            if (queryMagnitude == 0 || documentMagnitude == 0)
                return 0;
            
            return dotProduct / (Math.Sqrt(queryMagnitude) * Math.Sqrt(documentMagnitude));
        }
        
        /// <summary>
        /// Get Inverse Document Frequency for a term
        /// </summary>
        private double GetInverseDocumentFrequency(string term)
        {
            if (_idfCache.ContainsKey(term))
                return _idfCache[term];
            
            // Calculate IDF: log(total_documents / documents_containing_term)
            var documentsWithTerm = CountDocumentsContainingTerm(term);
            var idf = Math.Log((double)_totalDocuments / documentsWithTerm);
            
            _idfCache[term] = idf;
            return idf;
        }
        
        /// <summary>
        /// Calculate term frequency in a specific document
        /// </summary>
        private double CalculateTermFrequencyInDocument(string term, Document document)
        {
            // Count occurrences of term in document
            var termCount = CountTermOccurrences(term, document.Content);
            var totalTerms = CountTotalTerms(document.Content);
            
            // Return TF (can be normalized or raw count)
            return termCount; // Raw count for this example
        }
        
        // Helper methods (implement these based on your document store)
        private List<Document> GetAllDocuments() => new List<Document>();
        private int CountDocumentsContainingTerm(string term) => 1; // Implement based on your index
        private int CountTermOccurrences(string term, string content) => content.Split(' ').Count(w => w.ToLower().Contains(term.ToLower()));
        private int CountTotalTerms(string content) => content.Split(' ').Length;
    }
    
    /// <summary>
    /// Simplified TF-IDF example showing key integration points
    /// </summary>
    public class SimpleTFIDFExample
    {
        /// <summary>
        /// This shows the minimal code needed to use Query Parser with TF-IDF
        /// </summary>
        public void DemonstrateTFIDFIntegration(QueryRepresentation query)
        {
            Console.WriteLine("=== TF-IDF Integration with Query Parser ===");
            Console.WriteLine($"Original Query: {query.OriginalQuery}");
            Console.WriteLine($"Clean Terms: [{string.Join(", ", query.Terms)}]");
            Console.WriteLine("Term Frequencies:");
            
            foreach (var kvp in query.TermFrequency)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            
            // Now you can directly use these for TF-IDF:
            var queryVector = new Dictionary<string, double>();
            
            foreach (var term in query.Terms)
            {
                var tf = query.TermFrequency[term]; // TF from Query Parser
                var idf = 1.0; // You'll calculate this from your document collection
                var tfidf = tf * idf;
                
                queryVector[term] = tfidf;
                Console.WriteLine($"TF-IDF for '{term}': TF({tf}) × IDF({idf}) = {tfidf}");
            }
            
            Console.WriteLine("Query Vector ready for document similarity calculation!");
        }
    }
    
    /// <summary>
    /// Example of how Query Parser output simplifies TF-IDF implementation
    /// </summary>
    public class TFIDFBenefitsExample
    {
        public void ShowBenefits(QueryRepresentation query)
        {
            Console.WriteLine("\n=== Benefits of Query Parser for TF-IDF ===");
            
            // ✅ Benefit 1: Pre-calculated Term Frequencies
            Console.WriteLine("✅ Term Frequencies already calculated:");
            foreach (var kvp in query.TermFrequency)
            {
                Console.WriteLine($"   '{kvp.Key}': {kvp.Value}");
            }
            
            // ✅ Benefit 2: Clean, stemmed terms
            Console.WriteLine("✅ Terms are already processed:");
            Console.WriteLine($"   Original: {query.OriginalQuery}");
            Console.WriteLine($"   Processed: [{string.Join(", ", query.Terms)}]");
            
            // ✅ Benefit 3: No preprocessing needed
            Console.WriteLine("✅ Ready for TF-IDF calculation:");
            Console.WriteLine("   - No need to count terms in query");
            Console.WriteLine("   - No need to remove stopwords");
            Console.WriteLine("   - No need to handle case sensitivity");
            Console.WriteLine("   - No need to stem words");
            
            // ✅ Benefit 4: Direct vector construction
            Console.WriteLine("✅ Direct vector construction:");
            var vector = query.Terms.ToDictionary(term => term, term => query.TermFrequency[term]);
            Console.WriteLine($"   Vector: {string.Join(", ", vector.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");
        }
    }
    
    // Data structures
    public class Document
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
    
    public class DocumentResult
    {
        public Document Document { get; }
        public double Score { get; }
        
        public DocumentResult(Document document, double score)
        {
            Document = document;
            Score = score;
        }
    }
    
    public class SearchResults
    {
        public IEnumerable<DocumentResult> Results { get; }
        public int TotalCount { get; }
        
        public SearchResults(IEnumerable<DocumentResult> results)
        {
            Results = results;
            TotalCount = results.Count();
        }
    }
}

/*
QUICK TF-IDF INTEGRATION CHECKLIST:
====================================

✅ 1. Accept QueryRepresentation as input
✅ 2. Use query.Terms for clean, stemmed terms
✅ 3. Use query.TermFrequency for pre-calculated TF values
✅ 4. Build query vector: tf * idf for each term
✅ 5. Calculate document vectors using same terms
✅ 6. Use cosine similarity for ranking

KEY INTEGRATION POINTS:
=======================

1. query.TermFrequency gives you TF directly
2. query.Terms gives you clean terms for vector construction
3. No preprocessing needed - just plug into your TF-IDF algorithm
4. Consistent with document indexing (same stemming, same stopword removal)

EXAMPLE USAGE:
==============

var parser = QueryParserFactory.CreateDefault();
var queryResult = parser.Parse("machine learning algorithms");

var tfidfFunction = new TFIDFMatchingFunction(totalDocs, idfValues);
var searchResults = tfidfFunction.SearchWithTFIDF(queryResult);

// queryResult.Terms = ["machine", "learn", "algorithm"]
// queryResult.TermFrequency = {"machine": 1, "learn": 1, "algorithm": 1}
// Ready for TF-IDF calculation!
*/ 