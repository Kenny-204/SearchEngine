# Search Engine Admin UI

A modern React application for managing documents in a search engine system. This app provides an intuitive interface for uploading, viewing, and managing documents with real-time API integration.

## Features

- **Document Upload**: Drag & drop file upload with custom naming and tagging
- **Document Management**: View, download, and delete documents
- **Real-time API Integration**: Connects to backend API endpoints
- **Pagination**: Efficient document browsing with pagination support
- **Toast Notifications**: User-friendly success/error feedback
- **Responsive Design**: Modern UI with smooth animations

## API Endpoints

The app integrates with the following API endpoints:

### Upload Document (POST)
- **URL**: `http://localhost:5097/documents`
- **Method**: POST
- **Body**: FormData with file, fileName (optional), and tags (optional)
- **Response**: Document object with metadata

### Get Documents (GET)
- **URL**: `http://localhost:5097/documents?page={page}&pageSize={pageSize}`
- **Method**: GET
- **Response**: Documents array with pagination info

### Delete Document (DELETE)
- **URL**: `http://localhost:5097/documents/{id}`
- **Method**: DELETE
- **Response**: Success confirmation

## Document Structure

Each document contains:
- **id**: Unique identifier
- **title**: Document title
- **filePath**: Cloudinary file path
- **fileType**: File extension/type
- **keywords**: Array of extracted keywords
- **metadata**: Page count, word count, file size
- **createdAt**: Upload timestamp
- **indexedAt**: Indexing completion timestamp (null if pending)

## Getting Started

1. **Install Dependencies**:
   ```bash
   npm install
   ```

2. **Start Development Server**:
   ```bash
   npm run dev
   ```

3. **Configure API Base URL**:
   Update the `baseUrl` in `src/services/fileService.ts` if your API runs on a different port.

## Usage

1. **Upload Documents**:
   - Drag and drop files or click to browse
   - Optionally set custom file names
   - Add tags for better organization
   - Files are automatically uploaded to the backend

2. **View Documents**:
   - Browse uploaded documents with pagination
   - View document metadata (pages, words, size)
   - Check indexing status (pending/completed)
   - Download or view documents in browser

3. **Manage Documents**:
   - Delete documents with confirmation dialog
   - Navigate between pages efficiently
   - Real-time updates after operations

## Error Handling

The app includes comprehensive error handling:
- Network connectivity issues
- API response errors
- File upload failures
- User-friendly error messages with toast notifications

## Technologies Used

- **React 18** with TypeScript
- **Vite** for fast development
- **Tailwind CSS** for styling
- **Lucide React** for icons
- **Fetch API** for HTTP requests

## Development

The app is structured with:
- **Components**: Reusable UI components
- **Services**: API integration layer
- **Types**: TypeScript interfaces
- **State Management**: React hooks for local state

## API Requirements

Ensure your backend API:
- Accepts multipart/form-data for file uploads
- Returns proper pagination metadata
- Handles CORS for local development
- Provides meaningful error messages
- Supports the document structure outlined above

## Troubleshooting

### CORS Issues
If you encounter CORS errors like "Access to fetch has been blocked by CORS policy", ensure your backend API includes the following headers:

```
Access-Control-Allow-Origin: http://localhost:5173
Access-Control-Allow-Methods: GET, POST, DELETE, OPTIONS
Access-Control-Allow-Headers: Content-Type
```

For development, you can also use a CORS proxy or configure your backend to allow all origins during development.

### Network Errors
- Ensure your backend server is running on `http://localhost:5097`
- Check that the API endpoints are accessible
- Verify network connectivity between frontend and backend
