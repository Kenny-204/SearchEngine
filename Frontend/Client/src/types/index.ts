export interface SearchDocument {
  id: string;
  title: string;
  content: string;
  filename: string;
}

export interface Document {
  id: string;
  title: string;
  // content: string;
  filePath: string;
  fileType: string;
  keywords: string[];
  metadata: { [key: string]: string };
  createdAt: Date;
  indexedAt: Date;
}

export interface SearchResult {
  document: Document;
  relevanceScore: number;
  keywordStats: KeywordStats;
  preview: string;
}

export interface KeywordStats {
  [term: string]: number;
}

export interface AutocompleteMatch {
  text: string;
  type: 'prefix' | 'partial';
  document: string;
}