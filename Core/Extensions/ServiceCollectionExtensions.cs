using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using TESTPROJESI.Services.Implementations;
using TESTPROJESI.Services.Interfaces;
using TESTPROJESI.Business.Mappers;
using TESTPROJESI.Core.Mapping;
using Microsoft.Extensions.Options;
using TESTPROJESI.Core.Configuration;
using System.Text.Json;
using TESTPROJESI.Business.DTOs;

namespace TESTPROJESI.Core.Extensions
{
    /// <summary>
    /// 💉 Dependency Injection container için extension metodlar
    /// ✅ Mappers eklendi
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Tüm mapper'ları kaydeder
        /// </summary>
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            // ✅ Mapper'ları singleton olarak kaydet (state'siz, tekrar kullanılabilir)
            services.AddSingleton<IMapper<JsonElement, FinishedGoodsCreateDto>, FinishedGoodsMapper>();
            services.AddSingleton<IMapper<JsonElement, ProductionFlowDto>, ProductionFlowMapper>();
            services.AddSingleton<FinishedGoodsMapper>(); // Detay mapping için

            return services;
        }

        /// <summary>
        /// Tüm repository'leri kaydeder
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Generic repository
            services.AddScoped(typeof(TESTPROJESI.Repositories.Interfaces.IGenericRepository<>),
                             typeof(TESTPROJESI.Repositories.Implementations.GenericRepository<>));

            // Özel repository'ler
            services.AddScoped<TESTPROJESI.Repositories.Interfaces.IFinishedGoodsRepository,
                             TESTPROJESI.Repositories.Implementations.FinishedGoodsRepository>();

            return services;
        }

        /// <summary>
        /// Tüm servisleri kaydeder
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Token yönetimi
            services.AddScoped<TESTPROJESI.Services.Interfaces.ITokenManager,
                             TESTPROJESI.Services.Implementations.TokenManager>();

            // API servisleri
            services.AddScoped<TESTPROJESI.Services.Interfaces.IBaseApiService,
                             TESTPROJESI.Services.Implementations.BaseApiService>();

            services.AddScoped<TESTPROJESI.Services.Interfaces.INetOpenXService,
                             TESTPROJESI.Services.Implementations.NetOpenXService>();

            // Business servisleri
            services.AddScoped<TESTPROJESI.Services.Interfaces.IFinishedGoodsService,
                             TESTPROJESI.Services.Implementations.FinishedGoodsService>();

            services.AddScoped<TESTPROJESI.Services.Interfaces.IProductionFlowService,
                             TESTPROJESI.Services.Implementations.ProductionFlowService>();
            
            services.AddScoped<IProductionOrderService, ProductionOrderService>();

            return services;
        }

        /// <summary>
        /// HttpClient servislerini Polly ile birlikte kaydeder
        /// </summary>
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            // ✅ NetOpenXClient (iş emirleri dahil tüm NetOpenX çağrıları için)
            services.AddHttpClient("NetOpenXClient", (serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<NetOpenXSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
            })
            .AddPolicyHandler((serviceProvider, _) =>
                serviceProvider.GetRequiredService<IAsyncPolicy<HttpResponseMessage>>());

            // diğer servislerin client'ları (gerekirse)
            services.AddHttpClient<TESTPROJESI.Services.Implementations.NetOpenXService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddPolicyHandler((serviceProvider, _) =>
                serviceProvider.GetRequiredService<IAsyncPolicy<HttpResponseMessage>>());

            services.AddHttpClient<TESTPROJESI.Services.Implementations.BaseApiService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddPolicyHandler((serviceProvider, _) =>
                serviceProvider.GetRequiredService<IAsyncPolicy<HttpResponseMessage>>());

            return services;
        }

        /// <summary>
        /// Middleware'leri kaydeder
        /// </summary>
        public static IServiceCollection AddCustomMiddlewares(this IServiceCollection services)
        {
            // Middleware'ler burada singleton olarak kaydedilebilir
            // Şu an için gerekmiyor çünkü UseMiddleware ile ekleniyor
            return services;
        }

        /// <summary>
        /// Cache servislerini kaydeder
        /// </summary>
        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();

            return services;
        }
    }
}
