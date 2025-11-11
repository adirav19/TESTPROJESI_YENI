using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TESTPROJESI.Repositories.Interfaces;
using TESTPROJESI.Services.Interfaces;
using TESTPROJESI.Core.Extensions; // ✅ Extension metodlar

namespace TESTPROJESI.Repositories.Implementations
{
    /// <summary>
    /// 🎯 Generic Repository - Extension metodları kullanır
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IBaseApiService _apiService;
        protected readonly ITokenManager _tokenManager;
        protected readonly ILogger<GenericRepository<T>> _logger;
        protected readonly string _endpoint;

        public GenericRepository(
            IBaseApiService apiService,
            ITokenManager tokenManager,
            ILogger<GenericRepository<T>> logger,
            string endpoint)
        {
            _apiService = apiService;
            _tokenManager = tokenManager;
            _logger = logger;
            _endpoint = endpoint;
        }

        public virtual async Task<List<T>> GetAllAsync(string queryParams = null)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = string.IsNullOrEmpty(queryParams)
                    ? _endpoint
                    : $"{_endpoint}?{queryParams}";

                _logger.LogInformation("📋 GetAll: {Endpoint}", url);

                var result = await _apiService.GetAsync<JsonElement>(url, token);

                // ✅ Extension metodları kullan
                var data = result.UnwrapData();

                if (data.ValueKind == JsonValueKind.Array)
                {
                    return data.ToList<T>();
                }
                else if (data.ValueKind == JsonValueKind.Object)
                {
                    // Tek bir obje gelmiş, liste olarak döndür
                    var list = new List<T>();
                    var item = JsonSerializer.Deserialize<T>(data.GetRawText());
                    if (item != null) list.Add(item);
                    return list;
                }

                return new List<T>();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "❌ GetAll hatası: {Endpoint}", _endpoint);
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = $"{_endpoint}/{id}";

                _logger.LogInformation("🔍 GetById: {Url}", url);

                var result = await _apiService.GetAsync<JsonElement>(url, token);

                // ✅ Extension metodları kullan
                var data = result.UnwrapData();

                if (data.ValueKind == JsonValueKind.Object)
                {
                    return JsonSerializer.Deserialize<T>(data.GetRawText());
                }

                return null;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "❌ GetById hatası: {Id}", id);
                throw;
            }
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();

                _logger.LogInformation("➕ Create: {Endpoint}", _endpoint);

                var result = await _apiService.PostAsync<T>(_endpoint, entity, token);
                return result ?? entity;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "❌ Create hatası: {Endpoint}", _endpoint);
                throw;
            }
        }

        public virtual async Task<T> UpdateAsync(string id, T entity)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = $"{_endpoint}/{id}";

                _logger.LogInformation("✏️ Update: {Url}", url);

                var result = await _apiService.PutAsync<T>(url, entity, token);
                return result ?? entity;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "❌ Update hatası: {Id}", id);
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = $"{_endpoint}/{id}";

                _logger.LogInformation("🗑️ Delete: {Url}", url);

                return await _apiService.DeleteAsync(url, token);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "❌ Delete hatası: {Id}", id);
                throw;
            }
        }
    }
}