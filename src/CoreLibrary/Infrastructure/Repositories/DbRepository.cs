using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.Domain.Interfaces;
using CoreLibrary.Infrastructure.Persistence;

namespace CoreLibrary.Infrastructure.Repositories;

public class DbRepository : DbRepository<DataContext>, IDbRepository
{
    public DbRepository(DataContext dbContext) : base(dbContext)
    {
    }
}
