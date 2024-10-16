namespace SantanderTest.Contract.Infrastructure
{
    /// <summary>
    /// Cache service
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface ICacheService<TKey, TValue>
    {
        void Set(TKey key, TValue value);
        TValue Get(TKey key);
        bool Remove(TKey key);
        bool ContainsKey(TKey key);
    }
}