

using CoreLibrary.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.API.Domain.Interfaces.Repositories;

public interface IAuthRepository<TDbContext> where TDbContext : DbContext
{
    Task<User> Register(User user, string password);

    Task<User?> Login(string username, string password);

    Task<bool> UserExists(string username);
    Task<int> AddRole(string role, string description);
}
