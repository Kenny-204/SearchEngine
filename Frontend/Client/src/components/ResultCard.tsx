import React from 'react';
import { FileText } from 'lucide-react';
import { SearchResult } from '../types';

interface ResultCardProps {
  result: SearchResult;
  onViewDocument: (url: string) => void;
  query: string;
}

export const ResultCard: React.FC<ResultCardProps> = ({ result, onViewDocument, query }) => {
  const { document, keywordStats, preview } = result;

  return (
    <div 
      className="bg-white rounded-xl shadow-sm border border-slate-200 p-6 hover:shadow-md hover:border-indigo-200 transition-all cursor-pointer group"
      onClick={() => onViewDocument("https://res.cloudinary.com/dpfuj9km4/raw/upload/v1754998186/" + document.filePath)}
    >
      <div className="flex items-start gap-4">
        <div className="flex-shrink-0 p-3 bg-indigo-100 rounded-lg group-hover:bg-indigo-200 transition-colors">
          <FileText className="w-6 h-6 text-indigo-600" />
        </div>
        
        <div className="flex-grow">
          <h3 className="text-xl font-semibold text-slate-800 mb-2 group-hover:text-indigo-700 transition-colors">
            {document.title}
          </h3>
          
          <p className="text-slate-600 mb-4 leading-relaxed">
            {preview}
          </p>
          
          <div className="space-y-2">
            {Object.keys(keywordStats).length > 0 && (
              <div className="flex flex-wrap gap-2">
                {Object.entries(keywordStats).map(([term, count]) => (
                  <span 
                    key={term}
                    className="inline-flex items-center gap-1 px-3 py-1 bg-emerald-100 text-emerald-800 text-sm font-medium rounded-full"
                  >
                    <span className="font-semibold">"{term}"</span>
                    <span className="text-emerald-600">• {count} match{count > 1 ? 'es' : ''}</span>
                  </span>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};