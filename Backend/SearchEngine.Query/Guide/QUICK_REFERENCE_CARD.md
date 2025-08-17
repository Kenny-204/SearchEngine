# 🚀 Quick Reference Card for Matching Function development

## 📥 **What You Receive from Query Parser**

```csharp
QueryRepresentation query = parser.Parse("user input");
```

### **Key Properties:**
- **`query.Terms`** → `List<string>` - Clean, stemmed search terms
- **`query.TermFrequency`** → `Dictionary<string, int>` - Term counts for ranking
- **`query.OriginalQuery`** → `string` - Original user input for display
- **`query.HasStopwordsRemoved`** → `bool` - Processing metadata

## 🧮 **TF-IDF Algorithm Integration**

### **Perfect for TF-IDF! Your Query Parser gives you:**

```csharp
// 1. Query Term Frequency (TF) - Ready to use!
var queryTF = query.TermFrequency;
// Example: {"machine": 1, "learn": 1, "algorithm": 1}

// 2. Clean terms for vector construction
var terms = query.Terms;
// Example: ["machine", "learn", "algorithm"]

// 3. Build query vector for TF-IDF
public Dictionary<string, double> BuildQueryVector(QueryRepresentation query)
{
    var vector = new Dictionary<string, double>();
    
    foreach (var term in query.Terms)
    {
        // Get TF directly from Query Parser!
        var tf = query.TermFrequency[term];
        
        // Calculate TF-IDF: tf * idf
        var tfidf = tf * GetInverseDocumentFrequency(term);
        vector[term] = tfidf;
    }
    
    return vector;
}
```

### **TF-IDF Flow:**
```
Query Parser → TF-IDF Processing → Document Similarity
     ↓              ↓                    ↓
query.TermFreq → Query Vector → Cosine Similarity
query.Terms     Document Vector → Ranking Score
```

## 💡 **Quick Integration**

```csharp
public SearchResults Search(QueryRepresentation query)
{
    // Get clean search terms
    var searchTerms = query.Terms;
    
    // Get term frequencies for scoring
    var frequencies = query.TermFrequency;
    
    // Do your document matching here
    return MatchDocuments(searchTerms, frequencies);
}
```

## 🔍 **Example Input/Output**

**User types:** `"machine learning algorithms for data science"`

**You receive:**
```csharp
query.Terms = ["machine", "learn", "algorithm", "data", "science"]
query.TermFrequency = {
    "machine": 1,
    "learn": 1, 
    "algorithm": 1,
    "data": 1,
    "science": 1
}
```

**For TF-IDF, you get:**
- **Term Frequencies (TF)** - Already calculated! ✅
- **Clean Terms** - Ready for vector operations! ✅
- **No preprocessing needed** - Just plug into your algorithm! ✅

## ⚡ **Pro Tips**

1. **Use `query.Terms`** for document matching
2. **Use `query.TermFrequency`** for relevance scoring
3. **Use `query.OriginalQuery`** for user feedback
4. **Terms are already:** lowercase, stemmed, stopword-free
5. **No need to:** clean punctuation, remove stopwords, handle casing
6. **Perfect for TF-IDF:** Term frequencies are pre-calculated!

## 🧪 **Test Your Integration**

```csharp
// Test that you can consume Query Parser output
var parser = QueryParserFactory.CreateDefault();
var testQuery = parser.Parse("test query");
var results = yourMatchingFunction.Search(testQuery);

Assert.NotNull(results);
```

## 🎯 **TF-IDF Specific Benefits**

✅ **Pre-calculated TF** - No need to count terms in query
✅ **Stemmed Terms** - Consistent with document indexing  
✅ **Clean Data** - No stopwords interfere with calculations
✅ **Efficient Processing** - Ready for vector operations
✅ **Accurate Scoring** - Term frequencies directly usable in TF calculations

---

**📚 Full Documentation:** See `QUERY_PARSER_SPECIFICATION.md`
**💻 Code Examples:** See `MATCHING_FUNCTION_INTEGRATION_EXAMPLE.cs`
**❓ Questions?** Check the test files in `SearchEngine.Query.Tests/` 