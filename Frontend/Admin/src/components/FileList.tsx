import React from 'react';
import { Download, Trash2, Tag, File, Calendar, ExternalLink, ChevronLeft, ChevronRight, Clock, CheckCircle, AlertCircle, RefreshCw } from 'lucide-react';
import { Document, PaginationInfo } from '../types/file';
import { fileService } from '../services/fileService';

interface FileListProps {
  documents: Document[];
  pagination: PaginationInfo;
  onPageChange: (page: number) => void;
  onDelete: (id: string) => void;
  onDownload: (doc: Document) => void;
  onView: (doc: Document) => void;
  isLoading: boolean;
  onReload: () => void;
  hasError: boolean;
}

export const FileList: React.FC<FileListProps> = ({ 
  documents, 
  pagination, 
  onPageChange, 
  onDelete, 
  onDownload, 
  onView, 
  isLoading,
  onReload,
  hasError
}) => {
  if (isLoading) {
    return (
      <div className="bg-white rounded-2xl shadow-xl p-12 text-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
        <p className="text-gray-500 text-lg">Loading documents...</p>
      </div>
    );
  }

  if (hasError) {
    return (
      <div className="bg-white rounded-2xl shadow-xl p-12 text-center">
        <AlertCircle className="mx-auto text-red-300 mb-4" size={64} />
        <p className="text-red-600 text-lg font-semibold mb-2">Failed to load documents</p>
        <p className="text-gray-500 mb-6">There was an error connecting to the server. This might be due to:</p>
        <ul className="text-gray-500 text-sm mb-6 space-y-1">
          <li>• Backend server is not running</li>
          <li>• CORS policy restrictions</li>
          <li>• Network connectivity issues</li>
        </ul>
        <button
          onClick={onReload}
          className="inline-flex items-center px-6 py-3 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition-all duration-200 hover:scale-105 shadow-lg"
        >
          <RefreshCw className="mr-2" size={20} />
          Retry Connection
        </button>
      </div>
    );
  }

  if (documents.length === 0) {
    return (
      <div className="bg-white rounded-2xl shadow-xl p-12 text-center">
        <File className="mx-auto text-gray-300 mb-4" size={64} />
        <p className="text-gray-500 text-lg">No documents found</p>
        <p className="text-gray-400 mb-6">Upload your first document to get started</p>
        <button
          onClick={onReload}
          className="inline-flex items-center px-6 py-3 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition-all duration-200 hover:scale-105 shadow-lg"
        >
          <RefreshCw className="mr-2" size={20} />
          Refresh Documents
        </button>
      </div>
    );
  }

  const getIndexingStatus = (indexedAt: string | null) => {
    if (indexedAt === null) {
      return { status: 'Pending', color: 'text-yellow-600', icon: Clock };
    }
    return { status: fileService.formatDate(indexedAt), color: 'text-green-600', icon: CheckCircle };
  };

  const renderPagination = () => {
    const { currentPage, totalPages, hasNextPage, hasPreviousPage } = pagination;
    
    if (totalPages <= 1) return null;

    return (
      <div className="flex items-center justify-between bg-white rounded-xl shadow-lg p-4 mt-6">
        <div className="text-sm text-gray-500">
          Page {currentPage + 1} of {totalPages}
        </div>
        
        <div className="flex items-center space-x-2">
          <button
            onClick={() => onPageChange(currentPage - 1)}
            disabled={!hasPreviousPage}
            className={`p-2 rounded-lg transition-all duration-200 ${
              hasPreviousPage
                ? 'text-blue-600 hover:bg-blue-50 hover:scale-110'
                : 'text-gray-400 cursor-not-allowed'
            }`}
          >
            <ChevronLeft size={20} />
          </button>
          
          <button
            onClick={() => onPageChange(currentPage + 1)}
            disabled={!hasNextPage}
            className={`p-2 rounded-lg transition-all duration-200 ${
              hasNextPage
                ? 'text-blue-600 hover:bg-blue-50 hover:scale-110'
                : 'text-gray-400 cursor-not-allowed'
            }`}
          >
            <ChevronRight size={20} />
          </button>
        </div>
      </div>
    );
  };

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold text-gray-800">Documents</h2>
        <div className="text-sm text-gray-500">
          {pagination.totalCount} total documents
        </div>
      </div>
      
      <div className="grid gap-4">
        {documents.map((doc, index) => {
          const indexingStatus = getIndexingStatus(doc.indexedAt);
          const StatusIcon = indexingStatus.icon;
          
          return (
            <div
              key={doc.id}
            className="bg-white rounded-xl shadow-lg p-6 transition-all duration-300 hover:shadow-xl hover:scale-[1.02] animate-slideUp"
            style={{ animationDelay: `${index * 100}ms` }}
          >
            <div className="flex items-center justify-between">
              <div className="flex-1 min-w-0">
                <div 
                  className="flex items-center mb-2 cursor-pointer hover:text-blue-600 transition-colors"
                    onClick={() => onView(doc)}
                >
                  <File className="text-blue-500 mr-3 flex-shrink-0" size={24} />
                  <div className="min-w-0 flex-1">
                    <h3 className="text-lg font-semibold text-gray-800 truncate hover:text-blue-600 transition-colors">
                        {doc.title}
                      <ExternalLink className="inline ml-2 opacity-0 group-hover:opacity-100 transition-opacity" size={16} />
                    </h3>
                      <p className="text-sm text-gray-500 truncate">
                        Type: {doc.fileType.toUpperCase()}
                      </p>
                    </div>
                  </div>
                  
                  <div className="flex items-center text-sm text-gray-500 mb-3 space-x-4">
                    <div className="flex items-center">
                      <Calendar size={14} className="mr-1" />
                      <span>{fileService.formatDate(doc.createdAt)}</span>
                </div>
                
                    <div className="flex items-center">
                      <span>{fileService.formatFileSize(doc.metadata.fileSizeBytes)}</span>
                </div>

                    <div className="flex items-center">
                      <StatusIcon size={14} className={`mr-1 ${indexingStatus.color}`} />
                      <span className={indexingStatus.color}>{indexingStatus.status}</span>
                    </div>
                  </div>

                  {/* Metadata */}
                  <div className="grid grid-cols-3 gap-4 mb-3 text-sm">
                    <div className="text-gray-600">
                      <span className="font-medium">Pages:</span> {doc.metadata.pageCount}
                    </div>
                    <div className="text-gray-600">
                      <span className="font-medium">Words:</span> {doc.metadata.wordCount.toLocaleString()}
                    </div>
                    <div className="text-gray-600">
                      <span className="font-medium">Size:</span> {fileService.formatFileSize(doc.metadata.fileSizeBytes)}
                    </div>
                  </div>

                  {doc.keywords && doc.keywords.length > 0 && (
                  <div className="flex flex-wrap gap-1">
                      {doc.keywords.map((keyword) => (
                      <span
                          key={keyword}
                        className="inline-flex items-center px-2 py-1 bg-blue-100 text-blue-800 rounded-full text-xs font-medium"
                      >
                        <Tag size={10} className="mr-1" />
                          {keyword}
                      </span>
                    ))}
                  </div>
                )}
              </div>

              <div className="flex items-center space-x-2 ml-4">
                <button
                    onClick={() => onView(doc)}
                  className="p-2 text-green-600 hover:bg-green-50 rounded-lg transition-all duration-200 hover:scale-110"
                    title="View document"
                >
                  <ExternalLink size={20} />
                </button>
                
                <button
                    onClick={() => onDownload(doc)}
                  className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-all duration-200 hover:scale-110"
                    title="Download document"
                >
                  <Download size={20} />
                </button>
                
                <button
                    onClick={() => onDelete(doc.id)}
                  className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-all duration-200 hover:scale-110"
                    title="Delete document"
                >
                  <Trash2 size={20} />
                </button>
              </div>
            </div>
          </div>
          );
        })}
      </div>
      
      {renderPagination()}
    </div>
  );
};