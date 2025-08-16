import React from 'react';
import { Upload, FileText } from 'lucide-react';

interface TabNavigationProps {
  activeTab: 'upload' | 'files';
  onTabChange: (tab: 'upload' | 'files') => void;
  fileCount: number;
}

export const TabNavigation: React.FC<TabNavigationProps> = ({
  activeTab,
  onTabChange,
  fileCount,
}) => {
  return (
    <div className="bg-white rounded-2xl shadow-xl p-2 mb-8">
      <div className="flex space-x-2">
        <button
          onClick={() => onTabChange('upload')}
          className={`flex-1 flex items-center justify-center px-6 py-4 rounded-xl font-semibold transition-all duration-300 ${
            activeTab === 'upload'
              ? 'bg-blue-600 text-white shadow-lg transform scale-105'
              : 'text-gray-600 hover:bg-gray-50 hover:text-blue-600'
          }`}
        >
          <Upload className="mr-2" size={20} />
          Upload Files
        </button>
        
        <button
          onClick={() => onTabChange('files')}
          className={`flex-1 flex items-center justify-center px-6 py-4 rounded-xl font-semibold transition-all duration-300 relative ${
            activeTab === 'files'
              ? 'bg-blue-600 text-white shadow-lg transform scale-105'
              : 'text-gray-600 hover:bg-gray-50 hover:text-blue-600'
          }`}
        >
          <FileText className="mr-2" size={20} />
          View Files
          {fileCount > 0 && (
            <span
              className={`ml-2 px-2 py-1 rounded-full text-xs font-bold ${
                activeTab === 'files'
                  ? 'bg-white text-blue-600'
                  : 'bg-blue-100 text-blue-600'
              }`}
            >
              {fileCount}
            </span>
          )}
        </button>
      </div>
    </div>
  );
};