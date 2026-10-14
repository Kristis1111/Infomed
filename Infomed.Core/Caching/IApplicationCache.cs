namespace Infomed.Core.Caching;

public interface IApplicationCache
{
    Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan expiration, CancellationToken cancellationToken)
        where T : notnull;
}
