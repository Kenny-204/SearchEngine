import React, { useState, useEffect } from 'react';
import { TabNavigation } from './components/TabNavigation';
import { FileUpload } from './components/FileUpload';
import { FileList } from './components/FileList';
import { DeleteConfirmDialog } from './components/DeleteConfirmDialog';
import { fileService } from './services/fileService';
import { Document, FileUploadData, DocumentsResponse } from './types/file';
import { Cloud, Database, FileText, AlertCircle, CheckCircle } from 'lucide-react';

interface Toast {
  id: string;
  type: 'success' | 'error' | 'info';
  message: string;
}

function App() {
  const [activeTab, setActiveTab] = useState<'upload' | 'files'>('upload');
  const [documents, setDocuments] = useState<Document[]>([]);
  const [pagination, setPagination] = useState({
    currentPage: 0,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false,
  });
  const [isUploading, setIsUploading] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [hasError, setHasError] = useState(false);
  const [toasts, setToasts] = useState<Toast[]>([]);
  const [deleteDialog, setDeleteDialog] = useState<{
    isOpen: boolean;
    documentId: string;
    documentTitle: string;
  }>({
    isOpen: false,
    documentId: '',
    documentTitle: '',
  });

  useEffect(() => {
    loadDocuments();
  }, []);

  const addToast = (type: Toast['type'], message: string) => {
    const id = Date.now().toString();
    const newToast: Toast = { id, type, message };
    setToasts(prev => [...prev, newToast]);
    
    // Auto-remove toast after 5 seconds
    setTimeout(() => {
      setToasts(prev => prev.filter(toast => toast.id !== id));
    }, 5000);
  };

  const removeToast = (id: string) => {
    setToasts(prev => prev.filter(toast => toast.id !== id));
  };

  const loadDocuments = async (page: number = 0) => {
    setIsLoading(true);
    setHasError(false);
    try {
      const response: DocumentsResponse = await fileService.getAllDocuments(page + 1, pagination.pageSize);
      setDocuments(response.documents);
      setPagination(response.pagination);
    } catch (error) {
      console.error('Failed to load documents:', error);
      setHasError(true);
      addToast('error', 'Failed to load documents. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const handlePageChange = (page: number) => {
    loadDocuments(page);
  };

  const handleReload = () => {
    loadDocuments();
  };

  const handleFileUpload = async (data: FileUploadData) => {
    setIsUploading(true);
    try {
      await fileService.uploadFile(data);
      addToast('success', 'Document uploaded successfully!');
      loadDocuments(); // Reload documents
      setActiveTab('files'); // Switch to files tab after successful upload
    } catch (error) {
      console.error('Upload failed:', error);
      const errorMessage = error instanceof Error ? error.message : 'Upload failed. Please try again.';
      addToast('error', errorMessage);
    } finally {
      setIsUploading(false);
    }
  };

  const handleDelete = (documentId: string) => {
    const document = documents.find(d => d.id === documentId);
    if (document) {
      setDeleteDialog({
        isOpen: true,
        documentId,
        documentTitle: document.title,
      });
    }
  };

  const confirmDelete = async () => {
    try {
      await fileService.deleteDocument(deleteDialog.documentId);
      setDocuments(prev => prev.filter(d => d.id !== deleteDialog.documentId));
      addToast('success', 'Document deleted successfully!');
    } catch (error) {
      addToast('error', 'Failed to delete document. Please try again.');
    }
    setDeleteDialog({ isOpen: false, documentId: '', documentTitle: '' });
  };

  const cancelDelete = () => {
    setDeleteDialog({ isOpen: false, documentId: '', documentTitle: '' });
  };

  const handleDownload = (document: Document) => {
    try {
      fileService.downloadFile(document);
      addToast('success', 'Download started!');
    } catch (error) {
      addToast('error', 'Failed to download document. Please try again.');
    }
  };

  const handleView = (document: Document) => {
    try {
      fileService.viewFile(document);
    } catch (error) {
      addToast('error', 'Failed to open document. Please try again.');
    }
  };

  const renderToast = (toast: Toast) => {
    const icons = {
      success: <CheckCircle size={20} className="text-green-500" />,
      error: <AlertCircle size={20} className="text-red-500" />,
      info: <AlertCircle size={20} className="text-blue-500" />,
    };

    const colors = {
      success: 'bg-green-50 border-green-200 text-green-800',
      error: 'bg-red-50 border-red-200 text-red-800',
      info: 'bg-blue-50 border-blue-200 text-blue-800',
    };

    return (
      <div
        key={toast.id}
        className={`flex items-center p-4 border rounded-lg shadow-lg ${colors[toast.type]} animate-slideIn`}
      >
        {icons[toast.type]}
        <span className="ml-3 flex-1">{toast.message}</span>
        <button
          onClick={() => removeToast(toast.id)}
          className="ml-4 text-gray-400 hover:text-gray-600"
        >
          ×
        </button>
      </div>
    );
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50">
      <div className="container mx-auto px-4 py-8 max-w-6xl">
        {/* Header */}
        <div className="text-center mb-12">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-blue-100 rounded-full mb-4">
            <Database className="text-blue-600" size={32} />
          </div>
          <h1 className="text-4xl font-bold text-gray-800 mb-2">Search Engine Admin</h1>
          <p className="text-gray-600">Upload, organize, and manage your client side search experience with ease</p>
        </div>

        {/* Toast Notifications */}
        <div className="fixed top-4 right-4 z-50 space-y-2 max-w-md">
          {toasts.map(renderToast)}
        </div>

        {/* Tab Navigation */}
        <TabNavigation
          activeTab={activeTab}
          onTabChange={setActiveTab}
          fileCount={pagination.totalCount}
        />

        {/* Tab Content */}
        <div className="animate-fadeIn">
          {activeTab === 'upload' ? (
            <FileUpload onFileUpload={handleFileUpload} isUploading={isUploading} />
          ) : (
            <FileList
              documents={documents}
              pagination={pagination}
              onPageChange={handlePageChange}
              onDelete={handleDelete}
              onDownload={handleDownload}
              onView={handleView}
              isLoading={isLoading}
              onReload={handleReload}
              hasError={hasError}
            />
          )}
        </div>

        {/* Delete Confirmation Dialog */}
        <DeleteConfirmDialog
          isOpen={deleteDialog.isOpen}
          fileName={deleteDialog.documentTitle}
          onConfirm={confirmDelete}
          onCancel={cancelDelete}
        />
      </div>
    </div>
  );
}

export default App;