import React, { useState, useEffect, useRef } from 'react';
import { ArrowLeft, ChevronUp, ChevronDown } from 'lucide-react';
import { SearchDocument } from '../types';
import { TextProcessor } from '../utils/textProcessor';

interface DocumentViewProps {
  document: SearchDocument;
  searchTerms: string[];
  onBack: () => void;
}

export const DocumentView: React.FC<DocumentViewProps> = ({
  document,
  searchTerms,
  onBack
}) => {
  const [currentPositions, setCurrentPositions] = useState<{ [term: string]: number }>({});
  const contentRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    // Initialize positions for each search term
    const initialPositions: { [term: string]: number } = {};
    searchTerms.forEach(term => {
      initialPositions[term] = -1;
    });
    setCurrentPositions(initialPositions);
  }, [searchTerms]);

  const navigateToTerm = (term: string, direction: 'next' | 'prev' = 'next') => {
    if (!contentRef.current) return;

    const text = contentRef.current.textContent || '';
    const occurrences = TextProcessor.getAllOccurrences(text, term);
    
    if (occurrences.length === 0) return;

    const currentPos = currentPositions[term];
    let newIndex: number;

    if (direction === 'next') {
      const currentIndex = occurrences.findIndex(pos => pos > currentPos);
      newIndex = currentIndex === -1 ? 0 : currentIndex;
    } else {
      const currentIndex = occurrences.findIndex(pos => pos >= currentPos);
      newIndex = currentIndex <= 0 ? occurrences.length - 1 : currentIndex - 1;
    }

    const newPosition = occurrences[newIndex];
    setCurrentPositions(prev => ({ ...prev, [term]: newPosition }));

    // Find and scroll to the highlighted element
    const highlightedElements = contentRef.current.querySelectorAll('mark');
    for (const element of highlightedElements) {
      const elementText = element.textContent?.toLowerCase() || '';
      if (elementText === term.toLowerCase()) {
        const elementPosition = Array.from(text).slice(0, text.toLowerCase().indexOf(elementText, newPosition)).length;
        if (Math.abs(elementPosition - newPosition) < 10) {
          element.scrollIntoView({ behavior: 'smooth', block: 'center' });
          element.classList.add('ring-2', 'ring-indigo-400', 'ring-offset-1');
          setTimeout(() => {
            element.classList.remove('ring-2', 'ring-indigo-400', 'ring-offset-1');
          }, 2000);
          break;
        }
      }
    }
  };

  const getOccurrenceText = (term: string): string => {
    if (!contentRef.current) return '0/0';
    
    const text = contentRef.current.textContent || '';
    const occurrences = TextProcessor.getAllOccurrences(text, term);
    
    if (occurrences.length === 0) return '0/0';
    
    const currentPos = currentPositions[term];
    const currentIndex = occurrences.findIndex(pos => pos === currentPos);
    
    return `${currentIndex + 1}/${occurrences.length}`;
  };

  const highlightedContent = TextProcessor.highlightText(document.content, searchTerms);

  return (
    <div className="min-h-screen bg-slate-50">
      {/* Fixed Header */}
      <div className="sticky top-0 z-10 bg-white border-b border-slate-200 shadow-sm">
        <div className="max-w-4xl mx-auto px-4 py-4">
          <div className="flex items-center justify-between mb-4">
            <button
              onClick={onBack}
              className="flex items-center gap-2 text-slate-600 hover:text-indigo-600 transition-colors"
            >
              <ArrowLeft className="w-5 h-5" />
              Back to Results
            </button>
            <h1 className="text-2xl font-bold text-slate-800">
              {document.title}
            </h1>
          </div>

          {/* Search Term Navigation */}
          {searchTerms.length > 0 && (
            <div className="flex flex-wrap gap-3">
              {searchTerms.map((term) => (
                <div key={term} className="flex items-center gap-2 bg-slate-100 rounded-lg p-2">
                  <span className="font-medium text-slate-700">"{term}"</span>
                  <span className="text-sm text-slate-500">
                    {getOccurrenceText(term)}
                  </span>
                  <div className="flex gap-1">
                    <button
                      onClick={() => navigateToTerm(term, 'prev')}
                      className="p-1 rounded hover:bg-slate-200 transition-colors"
                      title="Previous occurrence"
                    >
                      <ChevronUp className="w-4 h-4 text-slate-600" />
                    </button>
                    <button
                      onClick={() => navigateToTerm(term, 'next')}
                      className="p-1 rounded hover:bg-slate-200 transition-colors"
                      title="Next occurrence"
                    >
                      <ChevronDown className="w-4 h-4 text-slate-600" />
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      {/* Document Content */}
      <div className="max-w-4xl mx-auto px-4 py-8">
        <div 
          ref={contentRef}
          className="bg-white rounded-xl shadow-sm border border-slate-200 p-8"
        >
          <div 
            className="prose prose-lg max-w-none text-slate-800 leading-relaxed"
            dangerouslySetInnerHTML={{ __html: highlightedContent }}
          />
        </div>
      </div>
    </div>
  );
};