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
        var user = await _dataContext.FirstOrDefaultAsync<User>(a => a.Username == username);
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
}
