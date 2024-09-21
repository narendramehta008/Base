using CoreLibrary.API.Extensions;
using CoreLibrary.Infrastructure.Persistence;
using System.Diagnostics.CodeAnalysis;
using CoreLibrary.Infrastructure.Repositories;
using CoreLibrary.Domain.Interfaces;
using Serilog;
using CoreLibrary.API.Domain.Utilities;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        ConfigureBootstrapLogger();
        try
        {
            var serilogConfiguration = ConfigUtility.SetConfigurationBuilder().Build();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ConfigureSqliteDbServices<DataContext>(builder.Configuration);
            builder.Services.ConfigureServices();
            builder.Services.AddControllers();

            builder.Services.AddScoped<IDbRepository, DbRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();

            var app = builder.Build();
            app.InitialiseDataContext<DataContext>();
            app.Configure(app.Services.GetService<IWebHostEnvironment>()!);
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception in Starting the Application");
            Log.Fatal("Application Failed to Start");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureBootstrapLogger()
    {
        //Initialize Bootstrap Logger
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(path: "../logs/BootstrapErrorLog.json", rollingInterval: RollingInterval.Day)
            .CreateBootstrapLogger();
    }
}