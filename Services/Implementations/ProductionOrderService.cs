using System.Net.Http;
using System.Text;
using System.Text.Json;
using TESTPROJESI.Business.ProductionOrder;
using TESTPROJESI.Models;
using TESTPROJESI.Services.Interfaces;

namespace TESTPROJESI.Services.Implementations
{
    public class ProductionOrderService : IProductionOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductionOrderService> _logger;
        private readonly ITokenManager _tokenManager; // 🔥 eklendi

        public ProductionOrderService(IHttpClientFactory factory, ILogger<ProductionOrderService> logger, ITokenManager tokenManager)
        {
            _httpClient = factory.CreateClient("NetOpenXClient");
            _logger = logger;
            _tokenManager = tokenManager;
        }

        public async Task<ApiResponse<string>> CreateProductionOrderAsync(ProductionOrderCreateDto dto)
        {
            try
            {
                // ✅ Token al
                var token = await _tokenManager.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // ✅ Doğru URL
                var response = await _httpClient.PostAsync("v2/ProductionOrder", content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("❌ İş emri oluşturulamadı: {Result}", result);
                    return ApiResponse<string>.ErrorResponse($"İş emri oluşturulamadı: {result}");
                }

                _logger.LogInformation("✅ İş emri başarıyla oluşturuldu: {Result}", result);
                return ApiResponse<string>.SuccessResponse("İş emri başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ İş emri API çağrısı hatası");
                return ApiResponse<string>.ErrorResponse($"İş emri oluşturulurken hata: {ex.Message}");
            }
        }
    }
}
