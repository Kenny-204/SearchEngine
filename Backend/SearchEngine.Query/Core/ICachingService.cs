using System;

namespace SearchEngine.Query.Core
{
    /// <summary>
    /// Generic interface for caching services
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key</typeparam>
    /// <typeparam name="TValue">The type of the cached value</typeparam>
    public interface ICachingService<TKey, TValue>
    {
        /// <summary>
        /// Attempts to retrieve a value from the cache
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="value">The cached value if found</param>
        /// <returns>True if the value was found, false otherwise</returns>
        bool TryGet(TKey key, out TValue value);

        /// <summary>
        /// Stores a value in the cache
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="value">The value to cache</param>
        void Set(TKey key, TValue value);

        /// <summary>
        /// Removes a value from the cache
        /// </summary>
        /// <param name="key">The cache key to remove</param>
        /// <returns>True if the key was found and removed, false otherwise</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Clears all cached values
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the current number of cached items
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the maximum number of items the cache can hold
        /// </summary>
        int MaxSize { get; }
    }
} 