using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CoreLibrary.Infrastructure.Repositories;

public interface ICacheRepository : ICacheRepository<DbContext>
{
}

public class CacheRepository(IMemoryCache memoryCache, IDbRepository<DataContext> repository) : CacheRepository<DataContext>(memoryCache, repository), ICacheRepository
{
}
