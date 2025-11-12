using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Models;

namespace TESTPROJESI.Services.Interfaces
{
    public interface IFinishedGoodsService
    {
        /// <summary>
        /// 📋 Tüm üretim fişlerini listeler
        /// </summary>
        Task<List<FinishedGoodsCreateDto>> GetAllAsync(string? queryParams = null);

        /// <summary>
        /// 🔍 Belirli bir fişin detayını getirir
        /// </summary>
        Task<FinishedGoodsDetailDto?> GetByIdAsync(string fisNo);

        /// <summary>
        /// ➕ Yeni üretim fişi oluşturur
        /// </summary>
        Task<ApiResponse<JsonElement>> CreateAsync(FinishedGoodsCreateDto dto);

        /// <summary>
        /// ✏️ Mevcut fişi günceller
        /// </summary>
        Task<ApiResponse<JsonElement>> UpdateAsync(FinishedGoodsUpdateDto dto);

        /// <summary>
        /// 🗑️ Fişi siler
        /// </summary>
        Task<ApiResponse<bool>> DeleteAsync(string fisNo);

        /// <summary>
        /// 🔢 Kalem miktarını günceller
        /// </summary>
        Task<ApiResponse<JsonElement>> UpdateQuantityAsync(KalemDto dto);
    }
}