using Microsoft.Extensions.Caching.Memory;

namespace Infomed.Core.Caching;

public class InMemoryApplicationCache(IMemoryCache memoryCache) : IApplicationCache
{
    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan expiration,
        CancellationToken cancellationToken)
        where T : notnull
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await memoryCache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = expiration;
            return await factory(cancellationToken);
        }) ?? throw new InvalidOperationException($"Cache factory returned null for key '{key}'.");
    }
}
