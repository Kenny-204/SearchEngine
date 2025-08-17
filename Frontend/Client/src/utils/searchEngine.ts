import { SearchDocument, SearchResult, KeywordStats, AutocompleteMatch } from '../types';

export class SearchEngine {
  private documents: SearchDocument[];

  constructor(documents: SearchDocument[]) {
    this.documents = documents;
  }

  search(query: string): SearchResult[] {
    const searchTerms = this.tokenizeQuery(query);
    if (searchTerms.length === 0) return [];

    const results: SearchResult[] = [];

    for (const document of this.documents) {
      const keywordStats = this.analyzeKeywords(document, searchTerms);
      const hasMatches = Object.keys(keywordStats.foundTerms).length > 0;

      if (hasMatches) {
        const relevanceScore = this.calculateRelevanceScore(keywordStats, searchTerms);
        const preview = this.generatePreview(document, searchTerms);

        results.push({
          document,
          relevanceScore,
          keywordStats,
          preview
        });
      }
    }

    return results.sort((a, b) => b.relevanceScore - a.relevanceScore);
  }

  getAutocomplete(input: string, maxResults: number = 8): AutocompleteMatch[] {
    if (input.length < 2) return [];

    const matches = new Set<AutocompleteMatch>();
    const inputLower = input.toLowerCase();

    // Extract all words from all documents
    const allWords = new Set<string>();
    this.documents.forEach(doc => {
      const words = this.tokenizeText(doc.content + ' ' + doc.title);
      words.forEach(word => allWords.add(word));
    });

    // Find prefix matches first (higher priority)
    for (const word of allWords) {
      if (matches.size >= maxResults) break;
      if (word.toLowerCase().startsWith(inputLower)) {
        const documentTitle = this.findWordInDocument(word);
        matches.add({
          text: word,
          type: 'prefix',
          document: documentTitle
        });
      }
    }

    // Find partial matches if we need more results
    if (matches.size < maxResults) {
      for (const word of allWords) {
        if (matches.size >= maxResults) break;
        if (word.toLowerCase().includes(inputLower) && !word.toLowerCase().startsWith(inputLower)) {
          const documentTitle = this.findWordInDocument(word);
          matches.add({
            text: word,
            type: 'partial',
            document: documentTitle
          });
        }
      }
    }

    return Array.from(matches)
      .sort((a, b) => {
        if (a.type !== b.type) {
          return a.type === 'prefix' ? -1 : 1;
        }
        return a.text.localeCompare(b.text);
      })
      .slice(0, maxResults);
  }

  private findWordInDocument(word: string): string {
    for (const doc of this.documents) {
      if (this.tokenizeText(doc.content + ' ' + doc.title).includes(word.toLowerCase())) {
        return doc.title;
      }
    }
    return '';
  }

  private tokenizeQuery(query: string): string[] {
    return query
      .toLowerCase()
      .split(/\s+/)
      .filter(term => term.length > 0)
      .map(term => term.replace(/[^\w]/g, ''));
  }

  private tokenizeText(text: string): string[] {
    return text
      .toLowerCase()
      .split(/\s+/)
      .map(word => word.replace(/[^\w]/g, ''))
      .filter(word => word.length > 0);
  }

  private analyzeKeywords(document: SearchDocument, searchTerms: string[]): KeywordStats {
    const documentText = (document.content + ' ' + document.title).toLowerCase();
    const foundTerms: { [term: string]: number } = {};
    const missingTerms: string[] = [];

    searchTerms.forEach(term => {
      const regex = new RegExp(`\\b${term.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')}\\b`, 'gi');
      const matches = documentText.match(regex);
      const count = matches ? matches.length : 0;

      if (count > 0) {
        foundTerms[term] = count;
      } else {
        missingTerms.push(term);
      }
    });

    return { foundTerms, missingTerms };
  }

  private calculateRelevanceScore(keywordStats: KeywordStats, searchTerms: string[]): number {
    const foundCount = Object.keys(keywordStats.foundTerms).length;
    const totalCount = Object.values(keywordStats.foundTerms).reduce((sum, count) => sum + count, 0);
    const foundRatio = foundCount / searchTerms.length;

    return foundRatio * 100 + totalCount;
  }

  private generatePreview(document: SearchDocument, searchTerms: string[]): string {
    const sentences = document.content.split(/[.!?]+/).filter(s => s.trim().length > 0);
    
    // Find the first sentence that contains any search term
    for (const sentence of sentences) {
      const lowerSentence = sentence.toLowerCase();
      for (const term of searchTerms) {
        if (lowerSentence.includes(term.toLowerCase())) {
          return sentence.trim() + '.';
        }
      }
    }

    // Fallback to first sentence
    return sentences[0]?.trim() + '.' || '';
  }

  getDocument(id: string): SearchDocument | undefined {
    return this.documents.find(doc => doc.id === id);
  }
}