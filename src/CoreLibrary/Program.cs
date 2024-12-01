using CoreLibrary.API.Extensions;
using CoreLibrary.Infrastructure.Persistence;
using System.Diagnostics.CodeAnalysis;
using CoreLibrary.Infrastructure.Repositories;
using Serilog;
using CoreLibrary.API.Domain.Utilities;
using CoreLibrary.Application.Interfaces;
using CoreLibrary.API.Domain.Extensions;
using CoreLibrary.Domain.Entities;

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
            builder.Services.ConfigureJwtServices(builder.Configuration);

            builder.Services.AddScoped<IDbRepository, DbRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();

            var app = builder.Build();
            app.InitialiseDataContext<DataContext>((providor) =>
            {
                var repo = providor.GetRequiredService<IDbRepository>();
                var summary = new Summary()
                {
                    Header = "Learning",
                    SummaryHeader = "Learning Summaries",

                };
                summary.Summaries = summary.FetchDatas() ?? [];
                repo.NestedInAddSave(summary, (item) => item.Summaries);
            });
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