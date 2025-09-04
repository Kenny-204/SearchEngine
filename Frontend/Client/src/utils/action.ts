import { SearchResult } from "../types";
import { publicAxios } from "./axios";

export const autoComplete = async (query: string) => {
  try {
    console.log('🔍 Autocomplete request for:', query);
    const res = await publicAxios
      .get(`autosuggest?prefix=${query}`, {
        timeout: 5000,
      })
      .then((res) => res.data);
    console.log('✅ Autocomplete response:', res);
    if (res.length == 0) return "";
    return res[0];
  } catch (error) {
    console.error('❌ Autocomplete error:', error);
    return "";
  }
};

interface SearchQueryResponse {
  page: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  items: SearchQueryDoc[];
  prevPageUrl: string | null;
  nextPageUrl: string | null;
}

interface SearchQueryDoc extends QueryDoc {
  matches: { term: string; frequency: number }[];
  score: number
}

interface QueryDoc {
  id: string;
  title: string;
  filePath: string;
  fileType: string;
  keywords: string[];
  metadata: { [key: string]: string };
  createdAt: string;
  indexedAt: string;
}

export const searchQuery: (query: string) => Promise<SearchResult[]> = async (
  query
) => {
  try {
    console.log('🔍 Search request for:', query);
    const res = await publicAxios
      .get<SearchQueryResponse>(`documents/search?q=${query}`)
      .then((res) => res.data);
    console.log('✅ Search response:', res);
    const results = res.items;
    if (results.length == 0) return [];
    return results.map((s) => {
      return {
        document: {
          id: s.id,
          title: s.title,
          filePath: s.filePath,
          fileType: s.fileType,
          keywords: s.keywords,
          metadata: s.metadata,
          createdAt: new Date(s.createdAt),
          indexedAt: new Date(s.indexedAt),
        },
        relevanceScore: s.score,
        keywordStats: Object.fromEntries(s.matches.map((m) => [m.term, m.frequency])),
        preview: "Document",
      } as SearchResult;
    });
  } catch (error) {
    console.error('❌ Search error:', error);
    return [];
  }
};


export const documentCount = async () => {
  try {
    console.log('🔍 Getting document count...');
    const res = await publicAxios
      .get('documents/count')
      .then((res) => res.data);
    console.log('✅ Document count response:', res);
    return res.count || 0;
  } catch (error) {
    console.error('❌ Document count error:', error);
    return 0;
  }
};
