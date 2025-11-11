using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using TESTPROJESI.Middlewares;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    public sealed class MiddlewareModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            // No services to configure
        }

        public void ConfigureApplication(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
        }
    }
}
