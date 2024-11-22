using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Repositories;
using CoreLibrary.Application.Interfaces;
using CoreLibrary.Infrastructure.Persistence;

namespace CoreLibrary.Infrastructure.Repositories;

public class AuthRepository : AuthRepository<DataContext>, IAuthRepository
{
    public AuthRepository(IDbRepository<DataContext> dbContext) : base(dbContext)
    {
    }
}
