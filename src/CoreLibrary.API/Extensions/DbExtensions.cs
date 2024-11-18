using CoreLibrary.API.Domain.Constants;
using CoreLibrary.API.Domain.Entities;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.API.Extensions;

public static class DbExtensions
{
    public static IHost InitialiseDataContext<TDataContext>(this IHost webHost) where TDataContext : DbContext
    {
        using (var scope = webHost.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TDataContext>>();
            var context = services.GetService<TDataContext>() ?? throw new NullReferenceException(nameof(TDataContext));
            var _authRepository = services.GetService<IAuthRepository<TDataContext>>() ?? throw new NullReferenceException(nameof(IAuthRepository<TDataContext>));

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TDataContext).Name);

                try
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();

                    if (!_authRepository.UserExists("admin").Result)
                    {
                        var adminId = _authRepository.AddRole(Admin, Admin).Result;
                        var userId = _authRepository.AddRole(GlobalConstants.User, GlobalConstants.User).Result;

                        _ = _authRepository.Register(new User()
                        {
                            Email = "admin@gmail.com",
                            Username = "admin",
                            FirstName = "Admin",
                            LastName = "User",
                            ProfileUrl = "https://wallpaperheart.com/wp-content/uploads/2018/07/cute-baby.jpg",
                            RoleId = adminId,
                        }, "admin@537").Result;

                        _ = _authRepository.Register(new User()
                        {
                            Email = "user@gmail.com",
                            Username = "user",
                            FirstName = "user",
                            ProfileUrl = "https://wallpaperheart.com/wp-content/uploads/2018/07/cute-baby.jpg",
                            RoleId = userId,
                        }, "user@123").Result;

                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TDataContext).Name);
                }

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TDataContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TDataContext).Name);
            }
        }
        return webHost;
    }
}
