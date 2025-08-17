import { useState } from "react";
import { SearchBar } from "../components/SearchBar";
import { SearchResults } from "../components/SearchResults";
import { DocumentView } from "../components/DocumentView";
import { AnimatedText } from "../components/AnimatedText";
import { SearchEngine } from "../utils/searchEngine";
import { documents } from "../data/documents";
import { SearchResult, AutocompleteMatch } from "../types";
import Three from "../components/model";
import Header from "../components/Header";
import { autoComplete, searchQuery } from "../utils/action";

type pageState = "landing" | "results" | "document";

const searchEngine = new SearchEngine(documents);

const animatedPhrases = [
  "Search through local documents instantly.",
  "No internet needed — completely offline.",
  "Smart autocomplete for faster results.",
  "Highlight and navigate through your matches.",
];

function SearchPage() {
  const [state, setState] = useState<pageState>("landing");
  const [currentQuery, setCurrentQuery] = useState("");
  const [searchResults, setSearchResults] = useState<SearchResult[]>([]);
  const [currentDocument, setCurrentDocument] = useState<string>("");
  const [searchTerms, setSearchTerms] = useState<string[]>([]);

  const handleSearch = async (query: string) => {
    const results = await searchQuery(query);
    setSearchResults(results);
    setCurrentQuery(query);
    setSearchTerms(
      query
        .toLowerCase()
        .split(/\s+/)
        .filter((term) => term.length > 0)
    );
    setState("results");
  };

  // const handleViewDocument = (documentId: string, query: string) => {
  //   setCurrentDocument(documentId);
  //   setCurrentQuery(query);
  //   setSearchTerms(
  //     query
  //       .toLowerCase()
  //       .split(/\s+/)
  //       .filter((term) => term.length > 0)
  //   );
  //   setState("document");
  // };

  const handleViewDocument = (documentUrl: string) => {
    window.open(documentUrl, '_blank');
  };
  

  const handleBackToLanding = () => {
    setState("landing");
    setCurrentQuery("");
    setSearchResults([]);
    setCurrentDocument("");
    setSearchTerms([]);
  };

  const handleBackToResults = () => {
    setState("results");
  };

  if (state === "results") {
    return (
      <SearchResults
        results={searchResults}
        query={currentQuery}
        onBack={handleBackToLanding}
        onViewDocument={handleViewDocument}
      />
    );
  }

  if (state === "document") {
    const document = searchEngine.getDocument(currentDocument);
    if (!document) {
      return <div>Document not found</div>;
    }

    return (
      <DocumentView
        document={document}
        searchTerms={searchTerms}
        onBack={handleBackToResults}
      />
    );
  }

  return (
    <>
      <Header />
      <Three />
      <div className="min-h-screen">
        <div className="flex flex-col justify-center min-h-screen px-4">
          <div className="h-full flex flex-col max-w-4xl mx-auto w-full text-center justify-between gap-y-[10rem]">
            <div className="">
              {/* Logo/Title */}
              <div className="mb-8">
                <h1 className="pass-through test text-[5rem] leading-10 font-bold text-slate-800 mb-4 font-display z-[-500]" data-content="MetaSeek">
                  MetaSeek
                </h1>
                <p className="relative text-xl text-slate-600">
                  Discover information in your local documents
                </p>
              </div>

              {/* Search Bar */}
              <SearchBar
                onSearch={handleSearch}
                onAutocomplete={autoComplete}
                large={true}
                className="relative"
              />
            </div>

            <div className="glass p-4 relative">
              {/* Animated Features */}
              <div className="mb-12">
                <AnimatedText phrases={animatedPhrases} />
              </div>

              {/* Stats */}
              <div className="flex-1 grid grid-cols-1 md:grid-cols-3 gap-8 max-w-2xl mx-auto">
                <div className="text-center">
                  <div className="text-3xl font-bold text-indigo-600 mb-2">
                    {documents.length}
                  </div>
                  <div className="text-slate-600">Documents</div>
                </div>
                <div className="text-center">
                  <div className="text-3xl font-bold text-emerald-600 mb-2">
                    100%
                  </div>
                  <div className="text-slate-600">Offline</div>
                </div>
                <div className="text-center">
                  <div className="text-3xl font-bold text-purple-600 mb-2">
                    0ms
                  </div>
                  <div className="text-slate-600">Load Time</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default SearchPage;
