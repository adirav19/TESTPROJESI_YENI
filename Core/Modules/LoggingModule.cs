using Microsoft.AspNetCore.Builder;
using Serilog;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Configures Serilog based logging.
    /// </summary>
    public sealed class LoggingModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("Logs/app_log_.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    shared: true,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            builder.Host.UseSerilog();
        }

        public void ConfigureApplication(WebApplication app)
        {
            // No-op
        }
    }
}
