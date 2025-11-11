using System.Text.Json;
using Microsoft.Extensions.Logging;
using TESTPROJESI.Core.Builders;
using TESTPROJESI.Core.Constants;
using TESTPROJESI.Core.Extensions;
using TESTPROJESI.Core.Mapping;
using TESTPROJESI.Models;
using TESTPROJESI.Services.Interfaces;

namespace TESTPROJESI.Services.Base
{
    /// <summary>
    /// 🎯 Generic Module Service
    /// Tüm modüller için ortak CRUD operasyonları
    /// </summary>
    public abstract class GenericModuleService<TDto> where TDto : class
    {
        protected readonly IBaseApiService _apiService;
        protected readonly ITokenManager _tokenManager;
        protected readonly ILogger _logger;
        protected readonly IMapper<JsonElement, TDto> _mapper;
        protected readonly string _endpoint;

        protected GenericModuleService(
            IBaseApiService apiService,
            ITokenManager tokenManager,
            ILogger logger,
            IMapper<JsonElement, TDto> mapper,
            string endpoint)
        {
            _apiService = apiService;
            _tokenManager = tokenManager;
            _logger = logger;
            _mapper = mapper;
            _endpoint = endpoint;
        }

        /// <summary>
        /// 📋 Tüm kayıtları listeler
        /// </summary>
        public virtual async Task<List<TDto>> GetAllAsync(string? queryParams = null)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();

                var url = string.IsNullOrWhiteSpace(queryParams)
                    ? ApiRequestBuilder.Create()
                        .WithEndpoint(_endpoint)
                        .WithLimit(50)
                        .BuildUrl()
                    : $"{_endpoint}?{queryParams}";

                var responseJson = await _apiService.GetAsync<JsonElement>(url, token);
                var dataArray = responseJson.UnwrapData();

                if (dataArray.ValueKind != JsonValueKind.Array)
                {
                    _logger.LogWarning("⚠️ Beklenmeyen JSON formatı: {Json}", responseJson.ToString());
                    return new List<TDto>();
                }

                var list = _mapper.MapList(dataArray.EnumerateArray()).ToList();
                _logger.LogInformation(AppConstants.SuccessMessages.Listed, list.Count);

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ GetAll hatası: {Endpoint}", _endpoint);
                throw;
            }
        }

        /// <summary>
        /// 🔍 ID'ye göre kayıt getirir
        /// </summary>
        public virtual async Task<TDto?> GetByIdAsync(string id)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = $"{_endpoint}/{id}";

                var responseJson = await _apiService.GetAsync<JsonElement>(url, token);
                var data = responseJson.UnwrapData();

                if (data.ValueKind != JsonValueKind.Object)
                {
                    _logger.LogWarning("⚠️ Kayıt bulunamadı: {Id}", id);
                    return null;
                }

                return _mapper.Map(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ GetById hatası: {Id}", id);
                return null;
            }
        }

        /// <summary>
        /// ➕ Yeni kayıt oluşturur
        /// </summary>
        public virtual async Task<ApiResponse<JsonElement>> CreateAsync(object dto)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var response = await _apiService.PostAsync<JsonElement>(_endpoint, dto, token);

                _logger.LogInformation("✅ Kayıt oluşturuldu: {Endpoint}", _endpoint);
                return ApiResponse<JsonElement>.SuccessResponse(
                    response,
                    string.Format(AppConstants.SuccessMessages.Created, "Kayıt")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Create hatası: {Endpoint}", _endpoint);
                return ApiResponse<JsonElement>.ErrorResponse(
                    string.Format(AppConstants.ErrorMessages.CreateFailed, "Kayıt"),
                    ex.Message
                );
            }
        }

        /// <summary>
        /// ✏️ Kayıt günceller
        /// </summary>
        public virtual async Task<ApiResponse<JsonElement>> UpdateAsync(string id, object dto)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = $"{_endpoint}/{id}";
                var response = await _apiService.PutAsync<JsonElement>(url, dto, token);

                _logger.LogInformation("✅ Kayıt güncellendi: {Id}", id);
                return ApiResponse<JsonElement>.SuccessResponse(
                    response,
                    string.Format(AppConstants.SuccessMessages.Updated, "Kayıt")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Update hatası: {Id}", id);
                return ApiResponse<JsonElement>.ErrorResponse(
                    string.Format(AppConstants.ErrorMessages.UpdateFailed, "Kayıt"),
                    ex.Message
                );
            }
        }

        /// <summary>
        /// 🗑️ Kayıt siler
        /// </summary>
        public virtual async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = $"{_endpoint}/{id}";
                var success = await _apiService.DeleteAsync(url, token);

                if (success)
                {
                    _logger.LogInformation("✅ Kayıt silindi: {Id}", id);
                    return ApiResponse<bool>.SuccessResponse(
                        true,
                        string.Format(AppConstants.SuccessMessages.Deleted, "Kayıt")
                    );
                }

                _logger.LogWarning("⚠️ Kayıt silinemedi: {Id}", id);
                return ApiResponse<bool>.ErrorResponse(
                    string.Format(AppConstants.ErrorMessages.DeleteFailed, "Kayıt"),
                    "API'den başarısız yanıt döndü"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Delete hatası: {Id}", id);
                return ApiResponse<bool>.ErrorResponse(
                    string.Format(AppConstants.ErrorMessages.DeleteFailed, "Kayıt"),
                    ex.Message
                );
            }
        }
    }
}
