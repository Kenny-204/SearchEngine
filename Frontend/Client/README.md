# MetaSeek Search Engine - Frontend

## 🚀 Quick Start

```bash
npm install
npm run dev
```

Visit `http://localhost:5173` to access the search interface.

## 📋 System Design Documentation

Access the comprehensive system design breakdown at: `http://localhost:5173/system-design`

This page includes:
- **Architecture Overview** - Layered architecture with clear separation of concerns
- **Functional Requirements** - Core features and search capabilities
- **Non-Functional Requirements** - Performance, security, scalability, and reliability
- **Technical Decisions** - Detailed rationale for technology choices
- **System Architecture** - Visual representation of components
- **Performance & Scalability** - Optimization strategies and scaling considerations
- **Security Considerations** - Best practices and security measures
- **Future Enhancements** - Planned features and infrastructure improvements

## 🏗️ Project Structure

```
Frontend/Client/
├── src/
│   ├── components/          # Reusable UI components
│   │   ├── Header.tsx      # Navigation header
│   │   ├── SearchBar.tsx   # Search input component
│   │   ├── SearchResults.tsx # Results display
│   │   ├── ResultCard.tsx  # Individual result cards
│   │   └── model/          # 3D background components
│   ├── pages/
│   │   ├── search.tsx      # Main search page
│   │   └── system-design.tsx # System design documentation
│   ├── utils/
│   │   ├── action.ts       # API integration functions
│   │   └── axios.ts        # HTTP client configuration
│   ├── App.tsx             # Main app component with routing
│   └── main.tsx            # Application entry point
├── public/                 # Static assets
└── package.json           # Dependencies and scripts
```

## 🎯 Key Features

- **Modern UI/UX** - Clean, responsive design with Tailwind CSS
- **3D Background** - Interactive Three.js animated scene
- **Real-time Search** - Instant search results with autocomplete
- **Document Management** - Upload, search, and view documents
- **System Design Page** - Comprehensive architecture documentation

## 🔧 Technology Stack

- **React 18** - Modern React with hooks
- **TypeScript** - Type-safe development
- **Tailwind CSS** - Utility-first CSS framework
- **Three.js** - 3D graphics and animations
- **Axios** - HTTP client for API communication
- **React Router** - Client-side routing

## 🌐 API Integration

The frontend communicates with the ASP.NET Core backend API:

- **Base URL**: `http://localhost:5097`
- **Endpoints**:
  - `GET /documents/search` - Search documents
  - `GET /autosuggest` - Autocomplete suggestions
  - `GET /documents/count` - Document count
  - `POST /documents/upload` - Upload documents
  - `DELETE /documents/{id}` - Delete documents

## 🎨 Design System

- **Color Palette**: Blue and purple gradients with clean whites
- **Typography**: Modern sans-serif fonts
- **Components**: Consistent card-based layout
- **Animations**: Smooth transitions and 3D effects