using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCaching.Extensions;

public static class CacheExtensions
{
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value) => SetAsync<T>(cache, key, value, new DistributedCacheEntryOptions());
    
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        return cache.SetAsync(key, value.Serialize(), options);
    }

    public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
    {
        var val = cache.Get(key);
        value = default;
        if (val is null) return false;
        value = val.Deserialize<T>();
        return true;
    }
}