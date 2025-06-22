using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Raftlabs.Library.Caching.Enums;
using Raftlabs.Library.Caching.Models;
using Raftlabs.Library.Caching.Services;
using Raftlabs.Library.Interfaces;
using System.Text.Json;

namespace Raftlabs.Library.Caching.Services.Implementations;
public class CacheService(
    IMemoryCache memoryCache,
    IDistributedCache distributedCache,
    IOptions<CacheOptions> options) : ICacheService, IApplicationService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly CacheOptions _options = options.Value;

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default)
    {
        var expiration = absoluteExpiration ?? TimeSpan.FromMinutes(_options.AbsoluteExpirationInMinutes);

        if (_options.CacheType == "Distributed")
        {
            var json = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration,
                SlidingExpiration = TimeSpan.FromMinutes(_options.SlidingExpirationInMinutes)
            };

            await _distributedCache.SetStringAsync(key, json, options, cancellationToken);
        }
        else
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration,
                SlidingExpiration = TimeSpan.FromMinutes(_options.SlidingExpirationInMinutes)
            };

            _memoryCache.Set(key, value, options);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_options.CacheType == "Distributed")
            await _distributedCache.RemoveAsync(key, cancellationToken);
        else
            _memoryCache.Remove(key);
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default)
    {
        var cachedValue = await TryGetAsync<T>(key, cancellationToken);
        if (cachedValue.Found)
        {
            return cachedValue.Value;
        }

        var value = await factory();

        if (value is not null)
        {
            await SetAsync(key, value, absoluteExpiration, cancellationToken);
        }

        return value!;
    }

    private async Task<TryGetResult<T>> TryGetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_options.CacheType == CacheType.Distributed.ToString())
        {
            var json = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(json))
                return new TryGetResult<T>(false, default);

            try
            {
                var value = JsonSerializer.Deserialize<T>(json);
                return new TryGetResult<T>(value is not null, value);
            }
            catch
            {
                return new TryGetResult<T>(false, default);
            }
        }
        else
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return new TryGetResult<T>(true, value);
            }

            return new TryGetResult<T>(false, default);
        }
    }

}

