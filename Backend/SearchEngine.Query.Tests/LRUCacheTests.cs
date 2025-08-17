using System;
using Xunit;
using SearchEngine.Query.Infrastructure;

namespace SearchEngine.Query.Tests
{
    public class LRUCacheTests
    {
        [Fact]
        public void Constructor_Should_Throw_For_Invalid_MaxSize()
        {
            Assert.Throws<ArgumentException>(() => new LRUCache<string, string>(0));
            Assert.Throws<ArgumentException>(() => new LRUCache<string, string>(-1));
        }
        
        [Fact]
        public void Constructor_Should_Create_Empty_Cache()
        {
            var cache = new LRUCache<string, string>(10);
            
            Assert.Equal(0, cache.Count);
            Assert.Equal(10, cache.MaxSize);
        }
        
        [Fact]
        public void Set_Should_Add_New_Item()
        {
            var cache = new LRUCache<string, string>(5);
            
            cache.Set("key1", "value1");
            
            Assert.Equal(1, cache.Count);
            Assert.True(cache.TryGet("key1", out var value));
            Assert.Equal("value1", value);
        }
        
        [Fact]
        public void Set_Should_Update_Existing_Item()
        {
            var cache = new LRUCache<string, string>(5);
            
            cache.Set("key1", "value1");
            cache.Set("key1", "value2");
            
            Assert.Equal(1, cache.Count);
            Assert.True(cache.TryGet("key1", out var value));
            Assert.Equal("value2", value);
        }
        
        [Fact]
        public void TryGet_Should_Return_False_For_NonExistent_Key()
        {
            var cache = new LRUCache<string, string>(5);
            
            Assert.False(cache.TryGet("nonexistent", out var value));
            Assert.Equal(default(string), value);
        }
        
        [Fact]
        public void TryGet_Should_Return_True_For_Existing_Key()
        {
            var cache = new LRUCache<string, string>(5);
            cache.Set("key1", "value1");
            
            Assert.True(cache.TryGet("key1", out var value));
            Assert.Equal("value1", value);
        }
        
        [Fact]
        public void Remove_Should_Return_False_For_NonExistent_Key()
        {
            var cache = new LRUCache<string, string>(5);
            
            Assert.False(cache.Remove("nonexistent"));
        }
        
        [Fact]
        public void Remove_Should_Return_True_And_Remove_Existing_Key()
        {
            var cache = new LRUCache<string, string>(5);
            cache.Set("key1", "value1");
            
            Assert.True(cache.Remove("key1"));
            Assert.Equal(0, cache.Count);
            Assert.False(cache.TryGet("key1", out var _));
        }
        
        [Fact]
        public void Clear_Should_Remove_All_Items()
        {
            var cache = new LRUCache<string, string>(5);
            cache.Set("key1", "value1");
            cache.Set("key2", "value2");
            
            cache.Clear();
            
            Assert.Equal(0, cache.Count);
            Assert.False(cache.TryGet("key1", out var _));
            Assert.False(cache.TryGet("key2", out var _));
        }
        
        [Fact]
        public void Cache_Should_Respect_MaxSize()
        {
            var cache = new LRUCache<string, string>(3);
            
            cache.Set("key1", "value1");
            cache.Set("key2", "value2");
            cache.Set("key3", "value3");
            cache.Set("key4", "value4"); // This should evict key1
            
            Assert.Equal(3, cache.Count);
            Assert.False(cache.TryGet("key1", out var _)); // Should be evicted
            Assert.True(cache.TryGet("key2", out var _));
            Assert.True(cache.TryGet("key3", out var _));
            Assert.True(cache.TryGet("key4", out var _));
        }
        
        [Fact]
        public void LRU_Eviction_Should_Remove_Least_Recently_Used()
        {
            var cache = new LRUCache<string, string>(3);
            
            cache.Set("key1", "value1");
            cache.Set("key2", "value2");
            cache.Set("key3", "value3");
            
            // Access key1 to make it most recently used
            cache.TryGet("key1", out var _);
            
            // Add key4, should evict key2 (least recently used)
            cache.Set("key4", "value4");
            
            Assert.Equal(3, cache.Count);
            Assert.True(cache.TryGet("key1", out var _)); // Should remain (most recently used)
            Assert.False(cache.TryGet("key2", out var _)); // Should be evicted
            Assert.True(cache.TryGet("key3", out var _));
            Assert.True(cache.TryGet("key4", out var _));
        }
        
        [Fact]
        public void Update_Should_Move_Item_To_Front()
        {
            var cache = new LRUCache<string, string>(3);
            
            cache.Set("key1", "value1");
            cache.Set("key2", "value2");
            cache.Set("key3", "value3");
            
            // Update key1, making it most recently used
            cache.Set("key1", "value1_updated");
            
            // Add key4, should evict key2 (least recently used)
            cache.Set("key4", "value4");
            
            Assert.Equal(3, cache.Count);
            Assert.True(cache.TryGet("key1", out var value1));
            Assert.Equal("value1_updated", value1);
            Assert.False(cache.TryGet("key2", out var _)); // Should be evicted
            Assert.True(cache.TryGet("key3", out var _));
            Assert.True(cache.TryGet("key4", out var _));
        }
        
        [Fact]
        public void Cache_Should_Handle_Generic_Types()
        {
            var cache = new LRUCache<int, DateTime>(2);
            var now = DateTime.Now;
            
            cache.Set(1, now);
            cache.Set(2, now.AddHours(1));
            
            Assert.True(cache.TryGet(1, out var value1));
            Assert.Equal(now, value1);
            Assert.True(cache.TryGet(2, out var value2));
            Assert.Equal(now.AddHours(1), value2);
        }
    }
} 