using Microsoft.AspNetCore.Builder;
using TESTPROJESI.Core.Extensions;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Registers MVC and caching related services.
    /// </summary>
    public sealed class MvcAndCachingModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddCaching();
        }

        public void ConfigureApplication(WebApplication app)
        {
            // No-op
        }
    }
}
