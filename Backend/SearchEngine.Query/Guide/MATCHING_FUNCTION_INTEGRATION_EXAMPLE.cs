// using System;
// using System.Collections.Generic;
// using System.Linq;
// using SearchEngine.Query.Core;

// // This is an example of how the Matching Function developer should integrate
// // with the Query Parser output. Copy and adapt this code as needed.

// namespace SearchEngine.Matching.Examples
// {
//     /// <summary>
//     /// Example implementation showing how to consume Query Parser output
//     /// </summary>
//     public class MatchingFunctionExample
//     {
//         /// <summary>
//         /// Basic search using Query Parser output
//         /// </summary>
//         public SearchResults BasicSearch(QueryRepresentation query)
//         {
//             // 1. Extract the clean search terms
//             var searchTerms = query.Terms;
            
//             // 2. Get term frequencies for ranking
//             var termFrequencies = query.TermFrequency;
            
//             // 3. Log what we received (for debugging)
//             Console.WriteLine($"Searching for terms: [{string.Join(", ", searchTerms)}]");
//             Console.WriteLine($"Term frequencies: {string.Join(", ", termFrequencies.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");
            
//             // 4. Perform your document matching logic here
//             var results = PerformDocumentMatching(searchTerms, termFrequencies);
            
//             return results;
//         }
        
//         /// <summary>
//         /// Advanced search with relevance scoring using term frequencies
//         /// </summary>
//         public SearchResults AdvancedSearchWithRanking(QueryRepresentation query)
//         {
//             var results = new List<DocumentResult>();
            
//             // Simulate getting documents from your document store
//             var documents = GetDocuments();
            
//             foreach (var document in documents)
//             {
//                 var score = CalculateDocumentScore(document, query);
                
//                 if (score > 0)
//                 {
//                     results.Add(new DocumentResult(document, score));
//                 }
//             }
            
//             // Return results sorted by relevance score
//             return new SearchResults(results.OrderByDescending(r => r.Score));
//         }
        
//         /// <summary>
//         /// Calculate document relevance score using Query Parser output
//         /// </summary>
//         private double CalculateDocumentScore(Document document, QueryRepresentation query)
//         {
//             var score = 0.0;
            
//             // Score based on term frequency and document relevance
//             foreach (var term in query.Terms)
//             {
//                 // Get how many times this term appears in the query
//                 var queryFrequency = query.TermFrequency[term];
                
//                 // Calculate how relevant this term is to the document
//                 var documentRelevance = CalculateTermRelevance(term, document);
                
//                 // Higher frequency terms get higher weight
//                 score += queryFrequency * documentRelevance;
//             }
            
//             return score;
//         }
        
//         /// <summary>
//         /// Example of how to handle different query types
//         /// </summary>
//         public SearchResults HandleDifferentQueryTypes(QueryRepresentation query)
//         {
//             // Check if stopwords were removed
//             if (query.HasStopwordsRemoved)
//             {
//                 Console.WriteLine("Query was processed to remove stopwords");
//             }
            
//             // Check query complexity
//             if (query.Terms.Count == 1)
//             {
//                 return HandleSingleTermQuery(query);
//             }
//             else if (query.Terms.Count <= 3)
//             {
//                 return HandleSimpleQuery(query);
//             }
//             else
//             {
//                 return HandleComplexQuery(query);
//             }
//         }
        
//         /// <summary>
//         /// Example of processing the original query for user feedback
//         /// </summary>
//         public void ProcessUserQuery(QueryRepresentation query)
//         {
//             // Always preserve the original query for user display
//             var userQuery = query.OriginalQuery;
            
//             // Show user what they searched for
//             Console.WriteLine($"You searched for: \"{userQuery}\"");
            
//             // Show what terms were actually used for search
//             var searchTerms = string.Join(", ", query.Terms);
//             Console.WriteLine($"Search terms: {searchTerms}");
            
//             // Show processing info
//             if (query.HasStopwordsRemoved)
//             {
//                 Console.WriteLine("Note: Common words were removed to improve search results");
//             }
//         }
        
//         // Helper methods (implement these based on your document store)
//         private List<Document> GetDocuments() => new List<Document>();
//         private double CalculateTermRelevance(string term, Document document) => 0.0;
//         private SearchResults HandleSingleTermQuery(QueryRepresentation query) => new SearchResults();
//         private SearchResults HandleSimpleQuery(QueryRepresentation query) => new SearchResults();
//         private SearchResults HandleComplexQuery(QueryRepresentation query) => new SearchResults();
//         private SearchResults PerformDocumentMatching(List<string> terms, Dictionary<string, int> frequencies) => new SearchResults();
//     }
    
//     // Example data structures (define these based on your needs)
//     public class Document
//     {
//         public string Id { get; set; } = string.Empty;
//         public string Content { get; set; } = string.Empty;
//         public string Title { get; set; } = string.Empty;
//     }
    
//     public class DocumentResult
//     {
//         public Document Document { get; }
//         public double Score { get; }
        
//         public DocumentResult(Document document, double score)
//         {
//             Document = document;
//             Score = score;
//         }
//     }
    
//     public class SearchResults
//     {
//         public IEnumerable<DocumentResult> Results { get; }
//         public int TotalCount { get; }
        
//         public SearchResults(IEnumerable<DocumentResult> results)
//         {
//             Results = results;
//             TotalCount = results.Count();
//         }
//     }
// }

// /*
// QUICK INTEGRATION CHECKLIST:
// ============================

// ✅ 1. Your Matching Function should accept QueryRepresentation as input
// ✅ 2. Use query.Terms to get clean, stemmed search terms
// ✅ 3. Use query.TermFrequency for relevance scoring
// ✅ 4. Use query.OriginalQuery for user feedback and logging
// ✅ 5. Check query.HasStopwordsRemoved to understand processing

// EXAMPLE USAGE:
// ==============

// var parser = QueryParserFactory.CreateDefault();
// var queryResult = parser.Parse("machine learning algorithms");

// var matchingFunction = new MatchingFunction();
// var searchResults = matchingFunction.Search(queryResult);

// // queryResult.Terms = ["machine", "learn", "algorithm"]
// // queryResult.TermFrequency = {"machine": 1, "learn": 1, "algorithm": 1}
// */ 