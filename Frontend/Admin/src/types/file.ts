export interface DocumentMetadata {
  pageCount: number;
  wordCount: number;
  fileSizeBytes: number;
}

export interface Document {
  id: string;
  title: string;
  filePath: string;
  fileType: string;
  keywords: string[];
  metadata: DocumentMetadata;
  createdAt: string;
  indexedAt: string | null;
}

export interface PaginationInfo {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface DocumentsResponse {
  documents: Document[];
  pagination: PaginationInfo;
}

export interface FileUploadData {
  file: File;
  fileName?: string;
  tags?: string[];
}

export interface UploadResponse {
  id: string;
  title: string;
  filePath: string;
  fileType: string;
  keywords: string[];
  metadata: DocumentMetadata;
  createdAt: string;
  indexedAt: string | null;
}