using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System;
using System.Net.Http;
using TESTPROJESI.Core.Extensions;
using TESTPROJESI.Core.Configuration;
using TESTPROJESI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════════════════════════
// 🔧 1️⃣ CONFIGURATION
// ═══════════════════════════════════════════════════════════════════

// Configuration bind (strongly typed settings)
builder.Services.Configure<NetOpenXSettings>(
    builder.Configuration.GetSection(NetOpenXSettings.SectionName));

builder.Services.Configure<HttpClientSettings>(
    builder.Configuration.GetSection(HttpClientSettings.SectionName));

// ═══════════════════════════════════════════════════════════════════
// 📝 2️⃣ LOGGING (SERILOG)
// ═══════════════════════════════════════════════════════════════════

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

// ═══════════════════════════════════════════════════════════════════
// 🌐 3️⃣ MVC & CACHING
// ═══════════════════════════════════════════════════════════════════

builder.Services.AddControllersWithViews();
builder.Services.AddCaching(); // Extension metod

// ═══════════════════════════════════════════════════════════════════
// ⚙️ 4️⃣ POLLY POLICY (RETRY + CIRCUIT BREAKER)
// ═══════════════════════════════════════════════════════════════════

var httpSettings = builder.Configuration
    .GetSection(HttpClientSettings.SectionName)
    .Get<HttpClientSettings>() ?? new HttpClientSettings();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, TimeSpan delay) =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .Or<TaskCanceledException>()
        .WaitAndRetryAsync(
            retryCount,
            retry => TimeSpan.FromSeconds(Math.Pow(2, retry)) + delay,
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                Log.Warning("🔄 Retry {RetryCount} - {Delay}s sonra tekrar denenecek...",
                    retryCount, timespan.TotalSeconds);
            });

var retryPolicy = GetRetryPolicy(httpSettings.RetryCount, httpSettings.RetryDelay);

// ═══════════════════════════════════════════════════════════════════
// 🌐 5️⃣ HTTP CLIENTS (POLLY İLE)
// ═══════════════════════════════════════════════════════════════════

builder.Services.AddHttpClients(retryPolicy); // Extension metod

// ═══════════════════════════════════════════════════════════════════
// 💉 6️⃣ DEPENDENCY INJECTION
// ═══════════════════════════════════════════════════════════════════

builder.Services.AddMappers();             // ✅ YENİ - Mapper'ları kaydet
builder.Services.AddRepositories();        // Extension metod
builder.Services.AddApplicationServices(); // Extension metod

// ═══════════════════════════════════════════════════════════════════
// 🚀 7️⃣ APPLICATION BUILD
// ═══════════════════════════════════════════════════════════════════

var app = builder.Build();

// ═══════════════════════════════════════════════════════════════════
// 🌍 8️⃣ MIDDLEWARE PIPELINE
// ═══════════════════════════════════════════════════════════════════

// Environment-specific middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Custom middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Standard middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ═══════════════════════════════════════════════════════════════════
// 🧭 9️⃣ ROUTING
// ═══════════════════════════════════════════════════════════════════

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ═══════════════════════════════════════════════════════════════════
// ✅ 🔟 APPLICATION START
// ═══════════════════════════════════════════════════════════════════

try
{
    Log.Information("🚀 Uygulama başlatılıyor...");
    Log.Information("📍 Environment: {Environment}", app.Environment.EnvironmentName);

    // NetOpenX ayarlarını validate et
    var netOpenXSettings = builder.Configuration
        .GetSection(NetOpenXSettings.SectionName)
        .Get<NetOpenXSettings>();

    netOpenXSettings?.Validate();
    Log.Information("✅ NetOpenX ayarları doğrulandı: {BaseUrl}", netOpenXSettings?.BaseUrl);
    Log.Information("✅ Mapper'lar kaydedildi");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Uygulama başlatılamadı!");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
