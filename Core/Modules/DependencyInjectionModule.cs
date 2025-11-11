using Microsoft.AspNetCore.Builder;
using TESTPROJESI.Core.Extensions;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Registers application specific services and repositories.
    /// </summary>
    public sealed class DependencyInjectionModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddMappers();
            builder.Services.AddRepositories();
            builder.Services.AddApplicationServices();
        }

        public void ConfigureApplication(WebApplication app)
        {
            // No-op
        }
    }
}
