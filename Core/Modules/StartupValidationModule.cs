using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using TESTPROJESI.Core.Configuration;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Performs startup validation and informational logging.
    /// </summary>
    public sealed class StartupValidationModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            // No services to configure
        }

        public void ConfigureApplication(WebApplication app)
        {
            Log.Information("🚀 Uygulama başlatılıyor...");
            Log.Information("📍 Environment: {Environment}", app.Environment.EnvironmentName);

            using var scope = app.Services.CreateScope();
            var netOpenXSettings = scope.ServiceProvider
                .GetRequiredService<IOptions<NetOpenXSettings>>()
                .Value;

            netOpenXSettings?.Validate();
            Log.Information("✅ NetOpenX ayarları doğrulandı: {BaseUrl}", netOpenXSettings?.BaseUrl);
            Log.Information("✅ Mapper'lar kaydedildi");
        }
    }
}
