using SantanderTest.Contract.Infrastructure;
using System.Collections.Concurrent;

namespace SantanderTest.Service.Infrastructure
{
    /// <summary>
    /// Cache service based on ConcurrentDictionary 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ConcurrentDictionaryCacheService<TKey, TValue> : ICacheService<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _cache = new();

        public void Set(TKey key, TValue value)
        {
            _cache[key] = value;
        }

        public TValue Get(TKey key)
        {
            _cache.TryGetValue(key, out var value);
            return value; // Returns default(TValue) if not found
        }

        public bool Remove(TKey key)
        {
            return _cache.TryRemove(key, out _);
        }

        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key);
        }
    }
}

