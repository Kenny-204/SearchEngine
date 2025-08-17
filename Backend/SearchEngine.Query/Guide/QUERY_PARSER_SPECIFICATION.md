# Query Parser Output Specification for Matching Function

## 🎯 **Purpose**
This document defines the exact output format that the Query Parser produces, which the Matching Function must consume to perform document search and ranking.

## 📊 **Output Data Structure**

### **Primary Output: QueryRepresentation Object**

```csharp
public class QueryRepresentation
{
    // The exact query as entered by the user
    public string OriginalQuery { get; set; }
    
    // Clean, processed search terms ready for matching
    public List<string> Terms { get; set; }
    
    // Whether stopwords were removed during processing
    public bool HasStopwordsRemoved { get; set; }
    
    // Frequency count of each term in the query
    public Dictionary<string, int> TermFrequency { get; set; }
}
```

## 🔍 **Example Outputs**

### **Example 1: Simple Query**
```csharp
// User Input: "The quick brown fox jumps over the lazy dog"

QueryRepresentation result = {
    OriginalQuery: "The quick brown fox jumps over the lazy dog",
    Terms: ["quick", "brown", "fox", "jump", "lazy", "dog"],
    HasStopwordsRemoved: true,
    TermFrequency: {
        "quick": 1,
        "brown": 1, 
        "fox": 1,
        "jump": 1,
        "lazy": 1,
        "dog": 1
    }
}
```

### **Example 2: Complex Query with Stemming**
```csharp
// User Input: "machine learning algorithms for data science"

QueryRepresentation result = {
    OriginalQuery: "machine learning algorithms for data science",
    Terms: ["machine", "learn", "algorithm", "data", "science"],
    HasStopwordsRemoved: true,
    TermFrequency: {
        "machine": 1,
        "learn": 1,
        "algorithm": 1,
        "data": 1,
        "science": 1
    }
}
```

### **Example 3: Query with Repeated Terms**
```csharp
// User Input: "search engine search optimization"

QueryRepresentation result = {
    OriginalQuery: "search engine search optimization",
    Terms: ["search", "engine", "search", "optim"],
    HasStopwordsRemoved: true,
    TermFrequency: {
        "search": 2,  // Note: "search" appears twice
        "engine": 1,
        "optim": 1
    }
}
```

## 🚀 **What the Matching Function Receives**

### **1. Clean Search Terms**
- **No stopwords** (removed automatically)
- **No punctuation** (cleaned automatically)  
- **No case sensitivity** (normalized to lowercase)
- **Stemmed words** (e.g., "running" → "run", "algorithms" → "algorithm")

### **2. Term Frequency Data**
- **Exact count** of how many times each term appears
- **Useful for ranking** (more frequent terms = higher importance)
- **Helps with relevance scoring**

### **3. Processing Metadata**
- **OriginalQuery**: For display purposes, error messages, logging
- **HasStopwordsRemoved**: To understand what processing occurred

## 🧮 **TF-IDF Algorithm Requirements**

### **What TF-IDF Needs from Query Parser:**

#### **1. Query Term Frequency (TF)**
```csharp
// From Query Parser: query.TermFrequency
var queryTermFrequencies = query.TermFrequency;
// Example: {"machine": 1, "learn": 1, "algorithm": 1}

// Calculate TF for each term in the query
foreach (var term in query.Terms)
{
    var tf = queryTermFrequencies[term]; // Term frequency in query
    // Use this for query vector calculation
}
```

#### **2. Clean, Stemmed Terms**
```csharp
// From Query Parser: query.Terms
var queryTerms = query.Terms;
// Example: ["machine", "learn", "algorithm"]

// These terms are already:
// - Stemmed (no need to process "learning" → "learn")
// - Lowercase (no case sensitivity issues)
// - Stopword-free (no noise in calculations)
```

#### **3. Query Vector Construction**
```csharp
// Build query vector for TF-IDF similarity calculation
public Dictionary<string, double> BuildQueryVector(QueryRepresentation query)
{
    var queryVector = new Dictionary<string, double>();
    
    foreach (var term in query.Terms)
    {
        // Get term frequency from query
        var tf = query.TermFrequency[term];
        
        // Calculate TF-IDF score for this term in the query
        var tfidf = CalculateQueryTFIDF(term, tf);
        
        queryVector[term] = tfidf;
    }
    
    return queryVector;
}
```

### **TF-IDF Calculation Flow:**

```
Query Parser Output → TF-IDF Processing → Document Similarity
        ↓                      ↓                    ↓
   query.Terms         Query Vector         Cosine Similarity
   query.TermFreq      Document Vector      Ranking Score
```

#### **Complete TF-IDF Integration Example:**
```csharp
public class TFIDFMatchingFunction
{
    public SearchResults SearchWithTFIDF(QueryRepresentation query)
    {
        // 1. Get clean terms and frequencies from Query Parser
        var queryTerms = query.Terms;
        var queryTermFrequencies = query.TermFrequency;
        
        // 2. Build query vector using TF-IDF
        var queryVector = BuildQueryVector(query, queryTermFrequencies);
        
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
    
    private Dictionary<string, double> BuildQueryVector(QueryRepresentation query, Dictionary<string, int> termFrequencies)
    {
        var vector = new Dictionary<string, double>();
        
        foreach (var term in query.Terms)
        {
            // Use term frequency from Query Parser
            var tf = termFrequencies[term];
            
            // Calculate TF-IDF for query term
            var tfidf = tf * GetInverseDocumentFrequency(term);
            vector[term] = tfidf;
        }
        
        return vector;
    }
}
```

### **Key Benefits for TF-IDF:**

✅ **Pre-calculated Term Frequencies** - No need to count terms in query
✅ **Stemmed Terms** - Consistent with document indexing
✅ **Clean Data** - No stopwords interfere with calculations
✅ **Efficient Processing** - Ready for vector operations
✅ **Accurate Scoring** - Term frequencies directly usable in TF calculations

## 💻 **Matching Function Integration Code**

### **Basic Usage**
```csharp
public class MatchingFunction
{
    public SearchResults Search(QueryRepresentation query)
    {
        // Access the clean search terms
        var searchTerms = query.Terms;
        
        // Get term frequencies for ranking
        var termFrequencies = query.TermFrequency;
        
        // Perform document matching
        var results = MatchDocuments(searchTerms, termFrequencies);
        
        return results;
    }
}
```

### **Advanced Usage with Term Frequency**
```csharp
public SearchResults SearchWithRanking(QueryRepresentation query)
{
    var results = new List<DocumentResult>();
    
    foreach (var document in GetAllDocuments())
    {
        var score = 0.0;
        
        // Score based on term frequency and document relevance
        foreach (var term in query.Terms)
        {
            var queryFrequency = query.TermFrequency[term];
            var documentRelevance = CalculateTermRelevance(term, document);
            
            // Higher frequency terms get higher weight
            score += queryFrequency * documentRelevance;
        }
        
        if (score > 0)
        {
            results.Add(new DocumentResult(document, score));
        }
    }
    
    return new SearchResults(results.OrderByDescending(r => r.Score));
}
```

## 🔧 **Key Benefits for Matching Function**

### **1. Semantic Understanding**
- **"running"** and **"runs"** both become **"run"**
- **"algorithms"** becomes **"algorithm"**
- **"internationalization"** becomes **"internation"**

### **2. Noise Reduction**
- **Stopwords removed**: "the", "and", "for", "in", "on", etc.
- **Clean data**: No punctuation, consistent formatting
- **Focused search**: Only meaningful terms remain

### **3. Ranking Ready**
- **Term frequencies** for importance weighting
- **Structured data** for efficient processing
- **Metadata** for debugging and logging

## 📋 **Matching Function Requirements**

### **Must Handle:**
1. **List<string> Terms**: Iterate through search terms
2. **Dictionary<string, int> TermFrequency**: Access term counts
3. **String OriginalQuery**: For user feedback and logging
4. **Boolean HasStopwordsRemoved**: For processing context

### **Should Use:**
1. **Term frequency** for relevance scoring
2. **Stemmed terms** for broader document matching
3. **Clean terms** for exact matching when needed

### **Can Ignore:**
1. **Original casing** (already normalized)
2. **Punctuation** (already removed)
3. **Stopwords** (already filtered)

## 🧪 **Testing Your Integration**

### **Test Query Parser Output**
```csharp
// Test that Query Parser produces expected format
var parser = QueryParserFactory.CreateDefault();
var result = parser.Parse("test query");

Assert.NotNull(result);
Assert.NotNull(result.Terms);
Assert.NotNull(result.TermFrequency);
Assert.True(result.Terms.Count > 0);
Assert.True(result.TermFrequency.Count > 0);
```

### **Test Matching Function Input**
```csharp
// Test that Matching Function can consume the output
var matchingFunction = new MatchingFunction();
var searchResults = matchingFunction.Search(queryResult);

Assert.NotNull(searchResults);
// Add your specific assertions for search results
```

## 📚 **Additional Resources**

- **Query Parser Tests**: See `SearchEngine.Query.Tests/` for comprehensive examples
- **Configuration Options**: `QueryParserConfiguration` for customization
- **Factory Methods**: `QueryParserFactory` for different parser configurations

## 🎯 **Next Steps for Matching Function**

1. **Consume QueryRepresentation** object from Query Parser
2. **Use Terms list** for document matching
3. **Leverage TermFrequency** for relevance scoring
4. **Implement document search** using the clean, stemmed terms
5. **Return ranked results** based on term matches and frequencies

---