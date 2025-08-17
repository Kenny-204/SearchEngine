import React from 'react';
import { ArrowLeft } from 'lucide-react';
import { SearchResult } from '../types';
import { ResultCard } from './ResultCard';

interface SearchResultsProps {
  results: SearchResult[];
  query: string;
  onBack: () => void;
  onViewDocument: (url: string) => void;
}

export const SearchResults: React.FC<SearchResultsProps> = ({
  results,
  query,
  onBack,
  onViewDocument
}) => {
  console.log("SearchResults rendered with query:", query);
  console.log("SearchResults results:", results);
  return (
    <div className="min-h-screen bg-slate-50">
      <div className="max-w-4xl mx-auto px-4 py-8">
        <div className="mb-8">
          <button
            onClick={onBack}
            className="flex items-center gap-2 text-slate-600 hover:text-indigo-600 transition-colors mb-4"
          >
            <ArrowLeft className="w-5 h-5" />
            Back to Search
          </button>
          
          <h1 className="text-3xl font-bold text-slate-800 mb-2">
            Search Results
          </h1>
          <p className="text-slate-600">
            Found <span className="font-semibold text-indigo-600">{results.length}</span> result{results.length !== 1 ? 's' : ''} for <span className="font-semibold">"{query}"</span>
          </p>
        </div>
        
        {results.length === 0 ? (
          <div className="text-center py-12">
            <div className="text-slate-400 mb-4">
              <svg className="w-16 h-16 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9.172 16.172a4 4 0 015.656 0M9 12h6m-6-4h6m2 5.291A7.962 7.962 0 0112 15c-2.34 0-4.467-.891-6.072-2.354l-.828.793-1.414-1.414L4.879 10.9A8 8 0 0112 5l.001.031A8 8 0 0119.931 13H18.72z" />
              </svg>
            </div>
            <h3 className="text-xl font-medium text-slate-700 mb-2">No results found</h3>
            <p className="text-slate-500 mb-4">
              Try adjusting your search terms or check for typos.
            </p>
          </div>
        ) : (
          <div className="space-y-6">
            {results.map((result) => (
              <ResultCard
                key={result.document.id}
                result={result}
                query={query}
                onViewDocument={onViewDocument}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  );
};