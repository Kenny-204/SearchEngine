import React from 'react';
import { Search, Database, Zap, Shield, Scale, Globe, Cpu, HardDrive, Network } from 'lucide-react';

const SystemDesign: React.FC = () => {
  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 to-blue-50">
      <div className="container mx-auto px-4 py-8">
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold text-gray-900 mb-4">System Design</h1>
          <p className="text-xl text-gray-600 max-w-3xl mx-auto">
            A comprehensive breakdown of the MetaSeek search engine architecture, 
            technical decisions, and system components
          </p>
        </div>

        {/* System Overview */}
        <section className="mb-16">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-6 flex items-center">
              <Globe className="mr-3 text-blue-600" />
              System Overview
            </h2>
            <div className="grid md:grid-cols-2 gap-8">
              <div>
                <h3 className="text-xl font-semibold text-gray-800 mb-4">Architecture Pattern</h3>
                <p className="text-gray-600 mb-4">
                  <strong>Layered Architecture</strong> with clear separation of concerns:
                </p>
                <ul className="space-y-2 text-gray-600">
                  <li>• <strong>Presentation Layer:</strong> React frontend with TypeScript</li>
                  <li>• <strong>API Layer:</strong> ASP.NET Core RESTful endpoints</li>
                  <li>• <strong>Business Logic:</strong> Search algorithms and document processing</li>
                  <li>• <strong>Data Layer:</strong> MongoDB + Redis + Cloudinary</li>
                </ul>
              </div>
              <div>
                <h3 className="text-xl font-semibold text-gray-800 mb-4">Technology Stack</h3>
                <div className="grid grid-cols-2 gap-4">
                  <div className="bg-blue-50 p-4 rounded-lg">
                    <h4 className="font-semibold text-blue-800">Frontend</h4>
                    <p className="text-sm text-blue-600">React + TypeScript + Tailwind</p>
                  </div>
                  <div className="bg-green-50 p-4 rounded-lg">
                    <h4 className="font-semibold text-green-800">Backend</h4>
                    <p className="text-sm text-green-600">ASP.NET Core + C#</p>
                  </div>
                  <div className="bg-purple-50 p-4 rounded-lg">
                    <h4 className="font-semibold text-purple-800">Database</h4>
                    <p className="text-sm text-purple-600">MongoDB + Redis</p>
                  </div>
                  <div className="bg-orange-50 p-4 rounded-lg">
                    <h4 className="font-semibold text-orange-800">Storage</h4>
                    <p className="text-sm text-orange-600">Cloudinary CDN</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        {/* Functional Requirements */}
        <section className="mb-16">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-6 flex items-center">
              <Search className="mr-3 text-green-600" />
              Functional Requirements
            </h2>
            <div className="grid md:grid-cols-2 gap-8">
              <div>
                <h3 className="text-xl font-semibold text-gray-800 mb-4">Core Features</h3>
                <ul className="space-y-3 text-gray-600">
                  <li className="flex items-start">
                    <span className="bg-green-100 text-green-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">✓</span>
                    <span><strong>Document Upload:</strong> Support for PDF, DOCX, PPTX, TXT files</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-green-100 text-green-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">✓</span>
                    <span><strong>Full-Text Search:</strong> TF-IDF algorithm with stemming and stopword removal</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-green-100 text-green-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">✓</span>
                    <span><strong>Autocomplete:</strong> Real-time search suggestions with Redis caching</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-green-100 text-green-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">✓</span>
                    <span><strong>Document Management:</strong> CRUD operations with Cloudinary integration</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-green-100 text-green-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">✓</span>
                    <span><strong>Background Processing:</strong> Asynchronous document indexing</span>
                  </li>
                </ul>
              </div>
                             <div>
                 <h3 className="text-xl font-semibold text-gray-800 mb-4">Search Capabilities</h3>
                 <ul className="space-y-3 text-gray-600">
                   <li className="flex items-start">
                     <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">🔍</span>
                     <span><strong>TF-IDF Ranking:</strong> Term frequency-inverse document frequency scoring</span>
                   </li>
                   <li className="flex items-start">
                     <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">🔍</span>
                     <span><strong>Query Parser:</strong> Advanced text processing with multiple strategies</span>
                   </li>
                   <li className="flex items-start">
                     <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">🔍</span>
                     <span><strong>Stemming:</strong> Porter Stemmer algorithm for word normalization</span>
                   </li>
                   <li className="flex items-start">
                     <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">🔍</span>
                     <span><strong>Stopword Removal:</strong> Eliminates common words for better relevance</span>
                   </li>
                   <li className="flex items-start">
                     <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">🔍</span>
                     <span><strong>Pagination:</strong> Efficient result pagination with metadata</span>
                   </li>
                   <li className="flex items-start">
                     <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">🔍</span>
                     <span><strong>Keyword Highlighting:</strong> Shows matched terms in results</span>
                   </li>
                 </ul>
               </div>
            </div>
          </div>
        </section>

        {/* Non-Functional Requirements */}
        <section className="mb-16">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-6 flex items-center">
              <Scale className="mr-3 text-purple-600" />
              Non-Functional Requirements
            </h2>
            <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-6">
              <div className="bg-purple-50 p-6 rounded-xl">
                <div className="flex items-center mb-3">
                  <Zap className="text-purple-600 mr-2" />
                  <h3 className="font-semibold text-purple-800">Performance</h3>
                </div>
                <ul className="text-sm text-purple-700 space-y-1">
                  <li>• Search response: &lt; 500ms</li>
                  <li>• Upload processing: &lt; 2s</li>
                  <li>• Autocomplete: &lt; 100ms</li>
                  <li>• Redis caching for speed</li>
                </ul>
              </div>
              <div className="bg-green-50 p-6 rounded-xl">
                <div className="flex items-center mb-3">
                  <Shield className="text-green-600 mr-2" />
                  <h3 className="font-semibold text-green-800">Security</h3>
                </div>
                <ul className="text-sm text-green-700 space-y-1">
                  <li>• CORS policy configuration</li>
                  <li>• Input validation & sanitization</li>
                  <li>• Secure file upload handling</li>
                  <li>• Environment variable protection</li>
                </ul>
              </div>
              <div className="bg-blue-50 p-6 rounded-xl">
                <div className="flex items-center mb-3">
                  <Cpu className="text-blue-600 mr-2" />
                  <h3 className="font-semibold text-blue-800">Scalability</h3>
                </div>
                <ul className="text-sm text-blue-700 space-y-1">
                  <li>• Horizontal scaling ready</li>
                  <li>• Background task queues</li>
                  <li>• Stateless API design</li>
                  <li>• Cloud-native architecture</li>
                </ul>
              </div>
              <div className="bg-orange-50 p-6 rounded-xl">
                <div className="flex items-center mb-3">
                  <HardDrive className="text-orange-600 mr-2" />
                  <h3 className="font-semibold text-orange-800">Reliability</h3>
                </div>
                <ul className="text-sm text-orange-700 space-y-1">
                  <li>• Error handling & logging</li>
                  <li>• Graceful degradation</li>
                  <li>• Data consistency checks</li>
                  <li>• Backup & recovery</li>
                </ul>
              </div>
            </div>
          </div>
        </section>

        {/* System Architecture Diagram */}
        <section className="mb-16">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-6 flex items-center">
              <Network className="mr-3 text-indigo-600" />
              System Architecture
            </h2>
            <div className="bg-gray-50 rounded-xl p-8">
              <div className="text-center mb-8">
                <h3 className="text-xl font-semibold text-gray-800 mb-4">High-Level Architecture</h3>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-8 items-center">
                  {/* Frontend Layer */}
                  <div className="bg-blue-100 p-6 rounded-xl">
                    <h4 className="font-semibold text-blue-800 mb-3">Frontend Layer</h4>
                    <div className="space-y-2 text-sm text-blue-700">
                      <div className="bg-blue-200 p-2 rounded">React + TypeScript</div>
                      <div className="bg-blue-200 p-2 rounded">Tailwind CSS</div>
                      <div className="bg-blue-200 p-2 rounded">Three.js (3D)</div>
                      <div className="bg-blue-200 p-2 rounded">Axios HTTP Client</div>
                    </div>
                  </div>

                  {/* API Gateway */}
                  <div className="bg-green-100 p-6 rounded-xl">
                    <h4 className="font-semibold text-green-800 mb-3">API Gateway</h4>
                    <div className="space-y-2 text-sm text-green-700">
                      <div className="bg-green-200 p-2 rounded">ASP.NET Core</div>
                      <div className="bg-green-200 p-2 rounded">RESTful Endpoints</div>
                      <div className="bg-green-200 p-2 rounded">CORS Policy</div>
                      <div className="bg-green-200 p-2 rounded">Swagger Docs</div>
                    </div>
                  </div>

                  {/* Backend Services */}
                                     <div className="bg-purple-100 p-6 rounded-xl">
                     <h4 className="font-semibold text-purple-800 mb-3">Backend Services</h4>
                     <div className="space-y-2 text-sm text-purple-700">
                       <div className="bg-purple-200 p-2 rounded">Document Processor</div>
                       <div className="bg-purple-200 p-2 rounded">Query Parser</div>
                       <div className="bg-purple-200 p-2 rounded">Search Engine</div>
                       <div className="bg-purple-200 p-2 rounded">Indexer</div>
                       <div className="bg-purple-200 p-2 rounded">Background Worker</div>
                     </div>
                   </div>
                </div>

                {/* Data Layer */}
                <div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-8">
                  <div className="bg-orange-100 p-6 rounded-xl">
                    <h4 className="font-semibold text-orange-800 mb-3">Primary Database</h4>
                    <div className="space-y-2 text-sm text-orange-700">
                      <div className="bg-orange-200 p-2 rounded">MongoDB</div>
                      <div className="bg-orange-200 p-2 rounded">Document Storage</div>
                      <div className="bg-orange-200 p-2 rounded">Inverted Index</div>
                      <div className="bg-orange-200 p-2 rounded">Metadata</div>
                    </div>
                  </div>

                  <div className="bg-red-100 p-6 rounded-xl">
                    <h4 className="font-semibold text-red-800 mb-3">Cache Layer</h4>
                    <div className="space-y-2 text-sm text-red-700">
                      <div className="bg-red-200 p-2 rounded">Redis</div>
                      <div className="bg-red-200 p-2 rounded">Autocomplete Cache</div>
                      <div className="bg-red-200 p-2 rounded">Search Results</div>
                      <div className="bg-red-200 p-2 rounded">Session Data</div>
                    </div>
                  </div>

                  <div className="bg-indigo-100 p-6 rounded-xl">
                    <h4 className="font-semibold text-indigo-800 mb-3">File Storage</h4>
                    <div className="space-y-2 text-sm text-indigo-700">
                      <div className="bg-indigo-200 p-2 rounded">Cloudinary</div>
                      <div className="bg-indigo-200 p-2 rounded">CDN Distribution</div>
                      <div className="bg-indigo-200 p-2 rounded">File Processing</div>
                      <div className="bg-indigo-200 p-2 rounded">URL Management</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        {/* Technical Decisions */}
        <section className="mb-16">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-6 flex items-center">
              <Database className="mr-3 text-teal-600" />
              Technical Decisions & Rationale
            </h2>
            <div className="space-y-8">
                             <div className="border-l-4 border-teal-500 pl-6">
                 <h3 className="text-xl font-semibold text-gray-800 mb-3">1. MongoDB as Primary Database</h3>
                 <div className="grid md:grid-cols-2 gap-6">
                   <div>
                     <h4 className="font-semibold text-teal-700 mb-2">Why MongoDB?</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Document-Oriented:</strong> Perfect for storing document metadata</li>
                       <li>• <strong>Flexible Schema:</strong> Easy to evolve as requirements change</li>
                       <li>• <strong>Inverted Index Support:</strong> Native support for search indexing</li>
                       <li>• <strong>Horizontal Scaling:</strong> Built-in sharding capabilities</li>
                       <li>• <strong>JSON-like Documents:</strong> Natural fit for document metadata</li>
                       <li>• <strong>Aggregation Pipeline:</strong> Powerful data processing capabilities</li>
                     </ul>
                   </div>
                   <div>
                     <h4 className="font-semibold text-teal-700 mb-2">Trade-offs</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Consistency:</strong> Eventual consistency model</li>
                       <li>• <strong>Complex Queries:</strong> Less powerful than SQL for joins</li>
                       <li>• <strong>Storage:</strong> Higher storage overhead</li>
                       <li>• <strong>Transaction Support:</strong> Limited multi-document transactions</li>
                     </ul>
                   </div>
                 </div>
                 
                 <div className="mt-6 bg-teal-50 p-6 rounded-xl">
                   <h4 className="font-semibold text-teal-800 mb-3">Database Schema Design</h4>
                   <div className="grid md:grid-cols-2 gap-6">
                     <div>
                       <h5 className="font-semibold text-teal-700 mb-2">Document Collection</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>_id:</strong> MongoDB ObjectId (primary key)</li>
                         <li>• <strong>title:</strong> Document display name</li>
                         <li>• <strong>filePath:</strong> Cloudinary URL for file access</li>
                         <li>• <strong>fileType:</strong> File extension (.pdf, .docx, etc.)</li>
                         <li>• <strong>keywords:</strong> Array of extracted keywords</li>
                         <li>• <strong>metadata:</strong> Embedded document metadata</li>
                         <li>• <strong>createdAt:</strong> Upload timestamp</li>
                         <li>• <strong>indexedAt:</strong> Last indexing timestamp</li>
                       </ul>
                     </div>
                     <div>
                       <h5 className="font-semibold text-teal-700 mb-2">Inverted Index Collection</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>_id:</strong> Term string (primary key)</li>
                         <li>• <strong>postings:</strong> Array of document references</li>
                         <li>• <strong>documentFrequency:</strong> Number of documents containing term</li>
                         <li>• <strong>totalOccurrences:</strong> Total term frequency across all docs</li>
                         <li>• <strong>lastUpdated:</strong> Last index update timestamp</li>
                       </ul>
                     </div>
                   </div>
                 </div>
               </div>

                             <div className="border-l-4 border-blue-500 pl-6">
                 <h3 className="text-xl font-semibold text-gray-800 mb-3">2. TF-IDF Search Algorithm</h3>
                 <div className="grid md:grid-cols-2 gap-6">
                   <div>
                     <h4 className="font-semibold text-blue-700 mb-2">Why TF-IDF?</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Proven Algorithm:</strong> Industry standard for text search</li>
                       <li>• <strong>Relevance Scoring:</strong> Balances term frequency and rarity</li>
                       <li>• <strong>Scalable:</strong> Works well with inverted indexes</li>
                       <li>• <strong>Interpretable:</strong> Easy to debug and optimize</li>
                       <li>• <strong>Mathematical Foundation:</strong> Well-understood statistical basis</li>
                       <li>• <strong>Efficient Implementation:</strong> O(log n) search complexity</li>
                     </ul>
                   </div>
                   <div>
                     <h4 className="font-semibold text-blue-700 mb-2">Implementation Details</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Porter Stemmer:</strong> Word normalization</li>
                       <li>• <strong>Stopword Removal:</strong> Eliminates noise</li>
                       <li>• <strong>Inverted Index:</strong> Fast term lookup</li>
                       <li>• <strong>Background Processing:</strong> Non-blocking indexing</li>
                       <li>• <strong>Cosine Similarity:</strong> Document-query comparison</li>
                       <li>• <strong>Score Normalization:</strong> Consistent ranking across queries</li>
                     </ul>
                   </div>
                 </div>
                 
                 <div className="mt-6 bg-blue-50 p-6 rounded-xl">
                   <h4 className="font-semibold text-blue-800 mb-3">TF-IDF Mathematical Foundation</h4>
                   <div className="grid md:grid-cols-2 gap-6">
                     <div>
                       <h5 className="font-semibold text-blue-700 mb-2">Term Frequency (TF)</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>Formula:</strong> TF(t,d) = count(t,d) / max(count(t',d))</li>
                         <li>• <strong>Purpose:</strong> Measures term importance within document</li>
                         <li>• <strong>Normalization:</strong> Prevents bias toward longer documents</li>
                         <li>• <strong>Range:</strong> 0 ≤ TF ≤ 1</li>
                       </ul>
                     </div>
                     <div>
                       <h5 className="font-semibold text-blue-700 mb-2">Inverse Document Frequency (IDF)</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>Formula:</strong> IDF(t) = log(N / df(t))</li>
                         <li>• <strong>Purpose:</strong> Measures term rarity across corpus</li>
                         <li>• <strong>Logarithm:</strong> Reduces impact of very rare terms</li>
                         <li>• <strong>Effect:</strong> Rare terms get higher weight</li>
                       </ul>
                     </div>
                   </div>
                   <div className="mt-4 p-4 bg-white rounded-lg">
                     <h5 className="font-semibold text-blue-700 mb-2">Final TF-IDF Score</h5>
                     <p className="text-sm text-gray-600">
                       <strong>Formula:</strong> TF-IDF(t,d) = TF(t,d) × IDF(t)
                     </p>
                     <p className="text-sm text-gray-600 mt-2">
                       This score balances local term importance (TF) with global term rarity (IDF), 
                       providing a robust measure of document relevance for search queries.
                     </p>
                   </div>
                 </div>
               </div>

                             <div className="border-l-4 border-green-500 pl-6">
                 <h3 className="text-xl font-semibold text-gray-800 mb-3">3. Redis for Caching</h3>
                 <div className="grid md:grid-cols-2 gap-6">
                   <div>
                     <h4 className="font-semibold text-green-700 mb-2">Why Redis?</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>In-Memory:</strong> Sub-millisecond response times</li>
                       <li>• <strong>Data Structures:</strong> Rich set of data types</li>
                       <li>• <strong>Persistence:</strong> RDB and AOF for data durability</li>
                       <li>• <strong>Clustering:</strong> Horizontal scaling support</li>
                       <li>• <strong>Atomic Operations:</strong> Thread-safe data manipulation</li>
                       <li>• <strong>Pub/Sub:</strong> Real-time messaging capabilities</li>
                     </ul>
                   </div>
                   <div>
                     <h4 className="font-semibold text-green-700 mb-2">Caching Strategy</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Autocomplete:</strong> 10-minute TTL</li>
                       <li>• <strong>Search Results:</strong> 5-minute TTL</li>
                       <li>• <strong>Document Metadata:</strong> 30-minute TTL</li>
                       <li>• <strong>LRU Eviction:</strong> Memory management</li>
                       <li>• <strong>Cache Warming:</strong> Pre-load popular queries</li>
                       <li>• <strong>Cache Invalidation:</strong> On document updates</li>
                     </ul>
                   </div>
                 </div>
                 
                 <div className="mt-6 bg-green-50 p-6 rounded-xl">
                   <h4 className="font-semibold text-green-800 mb-3">Redis Data Structure Usage</h4>
                   <div className="grid md:grid-cols-2 gap-6">
                     <div>
                       <h5 className="font-semibold text-green-700 mb-2">String Operations</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>Search Results:</strong> JSON serialized results</li>
                         <li>• <strong>Document Metadata:</strong> Serialized document info</li>
                         <li>• <strong>TTL Management:</strong> Automatic expiration</li>
                         <li>• <strong>Atomic Updates:</strong> Thread-safe modifications</li>
                       </ul>
                     </div>
                     <div>
                       <h5 className="font-semibold text-green-700 mb-2">Hash Operations</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>User Sessions:</strong> Session data storage</li>
                         <li>• <strong>Configuration:</strong> System settings cache</li>
                         <li>• <strong>Partial Updates:</strong> Field-level modifications</li>
                         <li>• <strong>Memory Efficiency:</strong> Reduced memory footprint</li>
                       </ul>
                     </div>
                   </div>
                   <div className="mt-4 p-4 bg-white rounded-lg">
                     <h5 className="font-semibold text-green-700 mb-2">Cache Key Patterns</h5>
                     <ul className="text-sm text-gray-600 space-y-1">
                       <li>• <strong>Autocomplete:</strong> <code>autocomplete:prefix</code></li>
                       <li>• <strong>Search Results:</strong> <code>search:query_hash</code></li>
                       <li>• <strong>Document Metadata:</strong> <code>doc:doc_id</code></li>
                       <li>• <strong>Query Cache:</strong> <code>query:query_hash</code></li>
                     </ul>
                   </div>
                 </div>
               </div>

                             <div className="border-l-4 border-purple-500 pl-6">
                 <h3 className="text-xl font-semibold text-gray-800 mb-3">4. Cloudinary for File Storage</h3>
                 <div className="grid md:grid-cols-2 gap-6">
                   <div>
                     <h4 className="font-semibold text-purple-700 mb-2">Why Cloudinary?</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>CDN Integration:</strong> Global content delivery</li>
                       <li>• <strong>File Processing:</strong> Automatic format conversion</li>
                       <li>• <strong>Security:</strong> Signed URLs and access control</li>
                       <li>• <strong>Scalability:</strong> Handles large file volumes</li>
                       <li>• <strong>API-First:</strong> RESTful API for all operations</li>
                       <li>• <strong>Transformations:</strong> On-the-fly file modifications</li>
                     </ul>
                   </div>
                   <div>
                     <h4 className="font-semibold text-purple-700 mb-2">File Handling</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Extension Preservation:</strong> Proper file type handling</li>
                       <li>• <strong>URL Management:</strong> Secure and accessible links</li>
                       <li>• <strong>Error Handling:</strong> Graceful upload failures</li>
                       <li>• <strong>Cleanup:</strong> Automatic file deletion</li>
                       <li>• <strong>Versioning:</strong> Automatic file versioning</li>
                       <li>• <strong>Metadata:</strong> Rich file metadata extraction</li>
                     </ul>
                   </div>
                 </div>
                 
                 <div className="mt-6 bg-purple-50 p-6 rounded-xl">
                   <h4 className="font-semibold text-purple-800 mb-3">Cloudinary Upload Process</h4>
                   <div className="grid md:grid-cols-2 gap-6">
                     <div>
                       <h5 className="font-semibold text-purple-700 mb-2">Upload Configuration</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>Resource Type:</strong> raw (for documents)</li>
                         <li>• <strong>Public ID:</strong> docs/filename.ext</li>
                         <li>• <strong>Use File Name:</strong> false (custom naming)</li>
                         <li>• <strong>Overwrite:</strong> false (prevent conflicts)</li>
                         <li>• <strong>Access Mode:</strong> public (readable URLs)</li>
                       </ul>
                     </div>
                     <div>
                       <h5 className="font-semibold text-purple-700 mb-2">URL Structure</h5>
                       <ul className="text-sm text-gray-600 space-y-1">
                         <li>• <strong>Base URL:</strong> https://res.cloudinary.com/cloud_name</li>
                         <li>• <strong>Resource Type:</strong> raw/upload</li>
                         <li>• <strong>Version:</strong> v1756685063</li>
                         <li>• <strong>Public ID:</strong> docs/filename.ext</li>
                         <li>• <strong>Format:</strong> Automatic detection</li>
                       </ul>
                     </div>
                   </div>
                   <div className="mt-4 p-4 bg-white rounded-lg">
                     <h5 className="font-semibold text-purple-700 mb-2">Example URL</h5>
                     <p className="text-sm text-gray-600 font-mono">
                       https://res.cloudinary.com/dpfuj9km4/raw/upload/v1756685063/docs/document.pdf
                     </p>
                   </div>
                 </div>
               </div>

                             <div className="border-l-4 border-indigo-500 pl-6">
                 <h3 className="text-xl font-semibold text-gray-800 mb-3">5. Query Parser Architecture</h3>
                 <div className="grid md:grid-cols-2 gap-6">
                   <div>
                     <h4 className="font-semibold text-indigo-700 mb-2">Why Custom Query Parser?</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Specialized Processing:</strong> Optimized for document search</li>
                       <li>• <strong>Flexible Configuration:</strong> Multiple parsing strategies</li>
                       <li>• <strong>Performance Tuning:</strong> Caching and optimization</li>
                       <li>• <strong>Integration Ready:</strong> Seamless TF-IDF integration</li>
                     </ul>
                   </div>
                   <div>
                     <h4 className="font-semibold text-indigo-700 mb-2">Processing Pipeline</h4>
                     <ul className="text-gray-600 space-y-1">
                       <li>• <strong>Tokenization:</strong> Splits query into terms</li>
                       <li>• <strong>Normalization:</strong> Case conversion and cleaning</li>
                       <li>• <strong>Stemming:</strong> Porter algorithm for word roots</li>
                       <li>• <strong>Stopword Removal:</strong> Eliminates noise words</li>
                     </ul>
                   </div>
                 </div>
                 
                                   <div className="mt-6 bg-indigo-50 p-6 rounded-xl">
                    <h4 className="font-semibold text-indigo-800 mb-3">Query Parser Configurations</h4>
                    <div className="grid md:grid-cols-3 gap-4">
                      <div className="bg-white p-4 rounded-lg">
                        <h5 className="font-semibold text-indigo-700 mb-2">Default Parser</h5>
                        <ul className="text-sm text-gray-600 space-y-1">
                          <li>• Basic stemming and stopword removal</li>
                          <li>• Balanced performance and accuracy</li>
                          <li>• Suitable for most use cases</li>
                        </ul>
                      </div>
                      <div className="bg-white p-4 rounded-lg">
                        <h5 className="font-semibold text-indigo-700 mb-2">Performance Optimized</h5>
                        <ul className="text-sm text-gray-600 space-y-1">
                          <li>• LRU cache for stemmed terms</li>
                          <li>• Reduced processing overhead</li>
                          <li>• High-throughput scenarios</li>
                        </ul>
                      </div>
                      <div className="bg-white p-4 rounded-lg">
                        <h5 className="font-semibold text-indigo-700 mb-2">Accuracy Optimized</h5>
                        <ul className="text-sm text-gray-600 space-y-1">
                          <li>• Enhanced stemming algorithms</li>
                          <li>• Comprehensive stopword lists</li>
                          <li>• Maximum search precision</li>
                        </ul>
                      </div>
                    </div>
                    
                    <div className="mt-6">
                      <h4 className="font-semibold text-indigo-800 mb-3">Processing Pipeline Details</h4>
                      <div className="grid md:grid-cols-2 gap-6">
                        <div>
                          <h5 className="font-semibold text-indigo-700 mb-2">Tokenization Process</h5>
                          <ul className="text-sm text-gray-600 space-y-1">
                            <li>• <strong>Regex Pattern:</strong> \W+ (non-word characters)</li>
                            <li>• <strong>Case Conversion:</strong> To lowercase</li>
                            <li>• <strong>Length Filtering:</strong> 1-50 characters</li>
                            <li>• <strong>Whitespace Handling:</strong> Trim and normalize</li>
                            <li>• <strong>Special Characters:</strong> Remove punctuation</li>
                          </ul>
                        </div>
                        <div>
                          <h5 className="font-semibold text-indigo-700 mb-2">Stemming Algorithm</h5>
                          <ul className="text-sm text-gray-600 space-y-1">
                            <li>• <strong>Algorithm:</strong> Porter Stemmer</li>
                            <li>• <strong>Language:</strong> English</li>
                            <li>• <strong>Steps:</strong> 5-step reduction process</li>
                            <li>• <strong>Examples:</strong> running → run, algorithms → algorithm</li>
                            <li>• <strong>Caching:</strong> LRU cache for performance</li>
                          </ul>
                        </div>
                      </div>
                      
                      <div className="mt-4 p-4 bg-white rounded-lg">
                        <h5 className="font-semibold text-indigo-700 mb-2">Query Processing Example</h5>
                        <div className="text-sm text-gray-600 space-y-2">
                          <p><strong>Input:</strong> "Machine Learning Algorithms"</p>
                          <p><strong>Tokenization:</strong> ["machine", "learning", "algorithms"]</p>
                          <p><strong>Stemming:</strong> ["machin", "learn", "algorithm"]</p>
                          <p><strong>Stopword Removal:</strong> ["machin", "learn", "algorithm"] (no stopwords)</p>
                          <p><strong>Output:</strong> QueryRepresentation with processed terms</p>
                        </div>
                      </div>
                    </div>
                  </div>
               </div>
            </div>
          </div>
        </section>

        {/* Performance Metrics */}
        <section className="mb-16">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-6 flex items-center">
              <Zap className="mr-3 text-yellow-600" />
              Performance & Scalability
            </h2>
            <div className="grid md:grid-cols-2 gap-8">
              <div>
                <h3 className="text-xl font-semibold text-gray-800 mb-4">Performance Optimizations</h3>
                <ul className="space-y-3 text-gray-600">
                  <li className="flex items-start">
                    <span className="bg-yellow-100 text-yellow-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">⚡</span>
                    <span><strong>Inverted Index:</strong> O(log n) search complexity</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-yellow-100 text-yellow-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">⚡</span>
                    <span><strong>Redis Caching:</strong> Sub-millisecond response times</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-yellow-100 text-yellow-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">⚡</span>
                    <span><strong>Background Processing:</strong> Non-blocking document indexing</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-yellow-100 text-yellow-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">⚡</span>
                    <span><strong>Pagination:</strong> Efficient result limiting</span>
                  </li>
                                     <li className="flex items-start">
                     <span className="bg-yellow-100 text-yellow-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">⚡</span>
                     <span><strong>Connection Pooling:</strong> Optimized database connections</span>
                   </li>
                   <li className="flex items-start">
                     <span className="bg-yellow-100 text-yellow-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">⚡</span>
                     <span><strong>Query Parser Caching:</strong> LRU cache for stemmed terms</span>
                   </li>
                </ul>
              </div>
              <div>
                <h3 className="text-xl font-semibold text-gray-800 mb-4">Scalability Considerations</h3>
                <ul className="space-y-3 text-gray-600">
                  <li className="flex items-start">
                    <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">📈</span>
                    <span><strong>Horizontal Scaling:</strong> Stateless API design</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">📈</span>
                    <span><strong>Database Sharding:</strong> MongoDB native support</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">📈</span>
                    <span><strong>Load Balancing:</strong> Multiple API instances</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">📈</span>
                    <span><strong>CDN Distribution:</strong> Global content delivery</span>
                  </li>
                  <li className="flex items-start">
                    <span className="bg-blue-100 text-blue-800 rounded-full px-2 py-1 text-xs font-semibold mr-3 mt-0.5">📈</span>
                    <span><strong>Microservices Ready:</strong> Modular architecture</span>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </section>

        {/* Security Considerations */}
        <section className="mb-16">
          <div className="bg-white rounded-2xl shadow-xl p-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-6 flex items-center">
              <Shield className="mr-3 text-red-600" />
              Security & Best Practices
            </h2>
            <div className="grid md:grid-cols-3 gap-6">
              <div className="bg-red-50 p-6 rounded-xl">
                <h3 className="font-semibold text-red-800 mb-3">Input Validation</h3>
                <ul className="text-sm text-red-700 space-y-2">
                  <li>• File type validation</li>
                  <li>• File size limits</li>
                  <li>• SQL injection prevention</li>
                  <li>• XSS protection</li>
                </ul>
              </div>
              <div className="bg-orange-50 p-6 rounded-xl">
                <h3 className="font-semibold text-orange-800 mb-3">Authentication & Authorization</h3>
                <ul className="text-sm text-orange-700 space-y-2">
                  <li>• Environment variable protection</li>
                  <li>• API key management</li>
                  <li>• CORS policy configuration</li>
                  <li>• Rate limiting (planned)</li>
                </ul>
              </div>
              <div className="bg-green-50 p-6 rounded-xl">
                <h3 className="font-semibold text-green-800 mb-3">Data Protection</h3>
                <ul className="text-sm text-green-700 space-y-2">
                  <li>• Secure file uploads</li>
                  <li>• Data encryption at rest</li>
                  <li>• Secure URL generation</li>
                  <li>• Audit logging</li>
                </ul>
              </div>
            </div>
          </div>
        </section>

        
      </div>
    </div>
  );
};

export default SystemDesign;
