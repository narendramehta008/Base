using CoreLibrary.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace CoreLibrary.API.Domain.Services.Repositories;

public interface ICacheRepository<TDbContext> where TDbContext : class
{
    void PopulatingCache<T>(IEnumerable<T> records) where T : class;

    void CacheRelativeLife<T>(string key, T data, TimeSpan? timeSpan = null);

    IEnumerable<T> TryGet<T>(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includes) where T : class;
}

public class CacheRepository<TDbContext> : ICacheRepository<TDbContext> where TDbContext : class
{
    private readonly IDbRepository<TDbContext> _repository;

    private readonly IMemoryCache _memoryCache;

    public CacheRepository(IMemoryCache memoryCache, IDbRepository<TDbContext> repository)
    {
        _repository = repository ?? throw new ArgumentNullException("repository");
        _memoryCache = memoryCache ?? throw new ArgumentNullException("memoryCache");
    }

    public void PopulatingCache<T>(IEnumerable<T> records) where T : class
    {
        CacheRelativeLife(typeof(T).Name, records);
    }

    public IEnumerable<T> TryGet<T>(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includes) where T : class
    {
        if (_memoryCache.TryGetValue<IEnumerable<T>>(typeof(T).Name, out IEnumerable<T> value))
        {
            return value;
        }

        if (expression == null)
        {
            expression = (T a) => true;
        }

        IQueryable<T> seed = _repository.Where(expression);
        seed = includes.Aggregate(seed, (IQueryable<T> current, Expression<Func<T, object>> includeProperty) => EntityFrameworkQueryableExtensions.Include(current, includeProperty));
        value = seed.ToList();
        PopulatingCache(value);
        return value;
    }

    public void CacheRelativeLife<T>(string key, T data, TimeSpan? timeSpan = null)
    {
        _memoryCache.Set(key, data, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = (timeSpan ?? new TimeSpan(24, 0, 0))
        });
    }
}