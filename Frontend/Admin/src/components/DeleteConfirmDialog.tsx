import React from 'react';
import { AlertTriangle, X } from 'lucide-react';

interface DeleteConfirmDialogProps {
  isOpen: boolean;
  fileName: string;
  onConfirm: () => void;
  onCancel: () => void;
}

export const DeleteConfirmDialog: React.FC<DeleteConfirmDialogProps> = ({
  isOpen,
  fileName,
  onConfirm,
  onCancel,
}) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 animate-fadeIn">
      <div className="bg-white rounded-2xl shadow-2xl p-6 w-full max-w-md mx-4 animate-scaleIn">
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center">
            <AlertTriangle className="text-red-500 mr-3" size={24} />
            <h3 className="text-lg font-semibold text-gray-800">Confirm Delete</h3>
          </div>
          <button
            onClick={onCancel}
            className="text-gray-400 hover:text-gray-600 transition-colors"
          >
            <X size={20} />
          </button>
        </div>
        
        <p className="text-gray-600 mb-6">
          Are you sure you want to delete <span className="font-semibold">"{fileName}"</span>?
          This action cannot be undone.
        </p>
        
        <div className="flex space-x-3">
          <button
            onClick={onCancel}
            className="flex-1 px-4 py-2 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300 transition-colors font-medium"
          >
            Cancel
          </button>
          <button
            onClick={onConfirm}
            className="flex-1 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors font-medium"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
};