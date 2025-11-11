using Microsoft.AspNetCore.Builder;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Configures endpoint routing.
    /// </summary>
    public sealed class RoutingModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            // No services to configure
        }

        public void ConfigureApplication(WebApplication app)
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
