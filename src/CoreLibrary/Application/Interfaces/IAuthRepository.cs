using CoreLibrary.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Application.Interfaces;

public interface IAuthRepository : IAuthRepository<DbContext>
{
}
