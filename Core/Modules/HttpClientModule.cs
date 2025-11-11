using Microsoft.AspNetCore.Builder;
using TESTPROJESI.Core.Extensions;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Registers HttpClient instances with resiliency policies.
    /// </summary>
    public sealed class HttpClientModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClients();
        }

        public void ConfigureApplication(WebApplication app)
        {
            // No-op
        }
    }
}
