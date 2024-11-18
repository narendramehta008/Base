using CoreLibrary.API.Domain.Entities;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.API.Repositories;
public class AuthRepository<TDbContext> : IAuthRepository<TDbContext> where TDbContext : DbContext
{
    private readonly IDbRepository<TDbContext> _dataContext;

    public AuthRepository(IDbRepository<TDbContext> dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<User?> Login(string username, string password)
    {
        var user = _dataContext.Where<User>(a => a.Username == username).Include(a => a.Role).FirstOrDefault();
        if (user == null)
            return null;

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            return null;
        return user;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int index = 0; index < computedHash.Length; index++)
        {
            if (computedHash[index] != passwordHash[index])
                return false;
        }
        return true;
    }

    public async Task<User> Register(User user, string password)
    {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _dataContext.AddSaveAsync(user);
        return user;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public async Task<bool> UserExists(string username)
    {
        var res = _dataContext.Where<User>(a => a.Username == username);
        return await res.AnyAsync();
    }

    public async Task<int> AddRole(string role, string description)
    {
        var res = _dataContext.Where<Role>(a => a.Code == role);
        if (res.Count() > 0)
            return res.First().Id;

        var roles = new Role() { Code = role, Description = description };
        await _dataContext.AddSaveAsync(roles);
        return roles.Id;
    }
}
