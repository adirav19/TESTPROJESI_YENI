using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TESTPROJESI.Core.Configuration;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Binds strongly typed configuration sections.
    /// </summary>
    public sealed class ConfigurationModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.Configure<NetOpenXSettings>(
                builder.Configuration.GetSection(NetOpenXSettings.SectionName));

            builder.Services.Configure<HttpClientSettings>(
                builder.Configuration.GetSection(HttpClientSettings.SectionName));
        }

        public void ConfigureApplication(WebApplication app)
        {
            // No-op
        }
    }
}
