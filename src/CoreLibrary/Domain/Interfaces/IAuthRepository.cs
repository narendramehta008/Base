using CoreLibrary.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Domain.Interfaces;

public interface IAuthRepository : IAuthRepository<DbContext>
{
}
