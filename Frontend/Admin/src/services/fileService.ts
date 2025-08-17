import { Document, DocumentsResponse, FileUploadData, UploadResponse } from '../types/file';

class FileService {
  private baseUrl = 'http://localhost:5097';

  async uploadFile(data: FileUploadData): Promise<UploadResponse> {
    const formData = new FormData();
    formData.append('file', data.file);
    
    if (data.fileName) {
      formData.append('fileName', data.fileName);
    }
    
    if (data.tags && data.tags.length > 0) {
      data.tags.forEach(tag => {
        if (tag.trim() !== '') {
          formData.append('tags', tag.trim());
        }
      });
    }

    try {
      const response = await fetch(`${this.baseUrl}/documents`, {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || `Upload failed with status: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      console.error('Upload error:', error);
      if (error instanceof TypeError && error.message.includes('fetch')) {
        throw new Error('Network error: Unable to connect to the server. Please check your connection and try again.');
      }
      throw error;
    }
  }

  async getAllDocuments(page: number = 1, pageSize: number = 10): Promise<DocumentsResponse> {
    try {
      const response = await fetch(
        `${this.baseUrl}/documents?page=${page}&pageSize=${pageSize}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || `Failed to fetch documents: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      console.error('Fetch documents error:', error);
      if (error instanceof TypeError && error.message.includes('fetch')) {
        throw new Error('Network error: Unable to connect to the server. Please check your connection and try again.');
      }
      throw error;
    }
  }

  async deleteDocument(id: string): Promise<boolean> {
    try {
      const response = await fetch(`${this.baseUrl}/documents/${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || `Failed to delete document: ${response.status}`);
      }

      return true;
    } catch (error) {
      console.error('Delete document error:', error);
      if (error instanceof TypeError && error.message.includes('fetch')) {
        throw new Error('Network error: Unable to connect to the server. Please check your connection and try again.');
      }
      throw error;
    }
  }

  downloadFile(doc: Document): void {
    const downloadUrl = `https://res.cloudinary.com/dpfuj9km4/raw/upload/${doc.filePath}`;
    const link = document.createElement('a');
    link.href = downloadUrl;
    link.download = doc.title;
    link.target = '_blank';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  viewFile(doc: Document): void {
    const viewUrl = `https://res.cloudinary.com/dpfuj9km4/raw/upload/${doc.filePath}`;
    window.open(viewUrl, '_blank');
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  }

  getIndexingStatus(indexedAt: string | null): { status: string; color: string } {
    if (indexedAt === null) {
      return { status: 'Pending', color: 'text-yellow-600' };
    }
    return { status: this.formatDate(indexedAt), color: 'text-green-600' };
  }
}

export const fileService = new FileService();