using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using TESTPROJESI.Core.Configuration;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Configures resiliency policies used by HttpClient registrations.
    /// </summary>
    public sealed class PollyModule : IStartupModule
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IAsyncPolicy<HttpResponseMessage>>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<HttpClientSettings>>().Value;

                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TaskCanceledException>()
                    .WaitAndRetryAsync(
                        settings.RetryCount,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + settings.RetryDelay,
                        onRetry: (outcome, timespan, retryCount, context) =>
                        {
                            Log.Warning("🔄 Retry {RetryCount} - {Delay}s sonra tekrar denenecek...",
                                retryCount, timespan.TotalSeconds);
                        });
            });
        }

        public void ConfigureApplication(WebApplication app)
        {
            // No-op
        }
    }
}
