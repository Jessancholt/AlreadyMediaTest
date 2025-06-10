namespace Test.Core.Services.Interfaces;

public interface ICacheService<TKey>
{
    Task<TValue> GetAsync<TValue>(TKey key, Func<Task<TValue>> action);

    Task<List<TValue>> GetAsync<TValue>(TKey key, Func<Task<List<TValue>>> action);

    void Invalidate(TKey key);
}
