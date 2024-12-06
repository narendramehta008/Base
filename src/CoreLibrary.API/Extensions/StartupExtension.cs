using CoreLibrary.API.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CoreLibrary.API.Repositories;
using CoreLibrary.API.Domain.Interfaces.Services;
using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.API.Strategy;
using CoreLibrary.API.Services.Mappings;

namespace CoreLibrary.API.Extensions;

public static class StartupExtension
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddControllers();//.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
        services.ConfigureSwaggerServices(true);
        services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));
        services.AddScoped(typeof(IAuthRepository<>), typeof(AuthRepository<>));
        services.AddTransient(typeof(ICacheRepository<>), typeof(CacheRepository<>));
        //services.AddTransient<IOperationServices, OperationServices>();
        //services.AddTransient<IOperationRepository, OperationRepository>();
        services.AddScoped<IApiServiceRepository, ApiServiceRepository>();
        services.AddScoped<IRestApiRepository, RestApiRepository>();

        services.AddTransient<Func<int, IOperationStrategy>>(serviceProvider => opType =>
        {
            return (IOperationStrategy)serviceProvider.GetService(OpsServiceMapper.GetService(opType))!;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddMemoryCache();
        services.AddScoped<TestStrategy>();
        return services;
    }


    public static IServiceCollection ConfigureSqliteDbServices<TRepo>(this IServiceCollection services
            , IConfiguration configuration, string defaultPath = "DefaultConnection") where TRepo : DbContext
    {
        services.AddDbContext<TRepo>(context =>
        {
            context.UseSqlite(configuration.GetConnectionString(defaultPath));
        });
        //services.AddSingleton<TRepo>();

        return services;
    }
    public static IServiceCollection ConfigureJwtServices(this IServiceCollection services
                    , IConfiguration configuration, string defaultPath = "AppSetting:Token")
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(defaultPath).Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                };
            });
        return services;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.ConfigureSwagger();
        app.UseCors(DefaultCors);

        app.UseHttpsRedirection();

        app.UseRouting();


        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers()
            //NOTE: only add when authentication is added in services
            .RequireAuthorization();
        });
    }

    public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services, bool addTokenBearer = false)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultCors, builder =>
            {
                builder.WithOrigins("http://localhost:4200",
                        "https://localhost:4200", "http://corelibrary.azurewebsites.net", "https://corelibrary.azurewebsites.net")
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
            });
        });

        services.AddSwaggerGen(s =>
        {
            if (addTokenBearer)
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }

            s.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreLibrary.API", Version = "v1" });
        });

        return services;
    }

    public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);
        });
        return app;
    }
}