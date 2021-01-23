using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CoreLibrary.API.Extensions.Startup
{
    public static class StartupExtension
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigureSwaggerServices();
            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.ConfigureSwagger();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreLibrary.API", Version = "v1" });
            });
            return services;
        }
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreLibrary.API v1"));
            return app;
        }

    }
}
