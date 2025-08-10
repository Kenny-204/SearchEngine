using System;
using System.Collections.Generic;
using SearchEngine.Query.Core;

namespace SearchEngine.Query.Infrastructure
{
    /// <summary>
    /// Least Recently Used (LRU) cache implementation
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key</typeparam>
    /// <typeparam name="TValue">The type of the cached value</typeparam>
    public class LRUCache<TKey, TValue> : ICachingService<TKey, TValue>
    {
        private readonly Dictionary<TKey, LinkedListNode<CacheItem>> _cache;
        private readonly LinkedList<CacheItem> _lruList;
        private readonly int _maxSize;

        public LRUCache(int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentException("MaxSize must be positive", nameof(maxSize));

            _maxSize = maxSize;
            _cache = new Dictionary<TKey, LinkedListNode<CacheItem>>();
            _lruList = new LinkedList<CacheItem>();
        }

        public bool TryGet(TKey key, out TValue value)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                // Move to front (most recently used)
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                value = node.Value.Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public void Set(TKey key, TValue value)
        {
            if (_cache.TryGetValue(key, out var existingNode))
            {
                // Update existing value
                existingNode.Value.Value = value;
                _lruList.Remove(existingNode);
                _lruList.AddFirst(existingNode);
            }
            else
            {
                // Add new item
                var newNode = new LinkedListNode<CacheItem>(new CacheItem { Key = key, Value = value });
                _cache[key] = newNode;
                _lruList.AddFirst(newNode);

                // Remove least recently used item if cache is full
                if (_cache.Count > _maxSize)
                {
                    var lruNode = _lruList.Last;
                    _lruList.RemoveLast();
                    _cache.Remove(lruNode.Value.Key);
                }
            }
        }

        public bool Remove(TKey key)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _cache.Remove(key);
                _lruList.Remove(node);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _cache.Clear();
            _lruList.Clear();
        }

        public int Count => _cache.Count;
        public int MaxSize => _maxSize;

        private class CacheItem
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
        }
    }
} 