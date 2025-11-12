using System.Text.Json;
using Microsoft.Extensions.Logging;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Business.Mappers;
using TESTPROJESI.Services.Interfaces;
using TESTPROJESI.Services.Base;
using TESTPROJESI.Core.Constants;

namespace TESTPROJESI.Services.Implementations
{
    /// <summary>
    /// 🏭 ProductionFlow Service - Refactored with Generic Base
    /// ✅ Mapper Pattern kullanır
    /// ✅ RequestBuilder ile esnek query oluşturur
    /// ✅ Generic base service'ten kalıtım alır
    /// </summary>
    public class ProductionFlowService : GenericModuleService<ProductionFlowDto>, IProductionFlowService
    {
        public ProductionFlowService(
            IBaseApiService apiService,
            ITokenManager tokenManager,
            ILogger<ProductionFlowService> logger)
            : base(
                  apiService,
                  tokenManager,
                  logger,
                  new ProductionFlowMapper(),
                  AppConstants.Endpoints.ProductionFlow,
                  new ModuleServiceOptions
                  {
                      DefaultSortField = "IsEmriNo",
                      DefaultSortDescending = true
                  })
        {
        }

        /// <summary>
        /// 🔍 ID'ye göre kayıt getir (tip dönüşümü ile)
        /// </summary>
        public async Task<ProductionFlowDto> GetByIdAsync(int id)
        {
            var result = await GetByIdAsync(id.ToString());
            return result ?? new ProductionFlowDto();
        }

        /// <summary>
        /// ➕ Yeni kayıt oluştur (base metodunu kullan)
        /// </summary>
        public async Task<JsonElement> CreateAsync(ProductionFlowDto dto)
        {
            var result = await base.CreateAsync(dto);
            return result.Data;
        }

        /// <summary>
        /// 🗑️ Sil (tip dönüşümü ile)
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id.ToString());
        }

        /// <summary>
        /// 🏭 ProductionFlow'dan Mamul Fişi oluştur
        /// </summary>
        public async Task<JsonElement> CreateFinishedGoodsReceiptAsync(FinishedGoodsReceiptParamDto param)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                
                // API endpoint'i oluştur
                var endpoint = $"{_endpoint}/ProductionFlowToFinishedGoodsReceipt";
                
                var response = await _apiService.PostAsync<JsonElement>(endpoint, param, token);
                
                _logger.LogInformation("✅ Mamul fişi başarıyla oluşturuldu");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Mamul fişi oluşturma hatası");
                throw;
            }
        }
    }
}
