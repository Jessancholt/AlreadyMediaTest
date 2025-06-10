using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Test.Core.Configurations;
using Test.Core.Services.Interfaces;
using Test.Shared.Exceptions;

namespace Test.Core.Services;

internal sealed class CacheService<TKey> : ICacheService<TKey>
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService<TKey>> _logger;

    private readonly CacheOptions _settings;

    public CacheService(
        IMemoryCache cache,
        IOptions<CacheOptions> options,
        ILogger<CacheService<TKey>> logger)
    {
        _cache = cache;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<TValue> GetAsync<TValue>(TKey key, Func<Task<TValue>> action)
    {
        try
        {
            TValue value;
            if (!_cache.TryGetValue(key, out value))
            {
                value = await action();
                _cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_settings.ExpirationTime));
            }

            return value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new CoreException("Failed to get data from cache", ex);
        }
    }

    public async Task<List<TValue>> GetAsync<TValue>(TKey key, Func<Task<List<TValue>>> action)
    {
        try
        {
            List<TValue> value;
            if (!_cache.TryGetValue(key, out value))
            {
                value = await action();
                _cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_settings.ExpirationTime));
            }

            return value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new CoreException("Failed to get list data from cache", ex);
        }
    }

    public void Invalidate(TKey key)
    {
        try
        {
            _cache.Remove(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new CoreException("Failed to get list data from cache", ex);
        }
    }
}
