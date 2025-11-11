using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TESTPROJESI.Business.DTOs;

namespace TESTPROJESI.Services.Interfaces
{
    public interface IProductionFlowService
    {
        /// <summary>
        /// 📋 Tüm ProductionFlow kayıtlarını getirir
        /// </summary>
        Task<List<ProductionFlowDto>> GetAllAsync(string queryParams = null);

        /// <summary>
        /// 🔍 ID'ye göre tek bir ProductionFlow kaydı getirir
        /// </summary>
        Task<ProductionFlowDto> GetByIdAsync(int id);

        /// <summary>
        /// ➕ Yeni ProductionFlow kaydı oluşturur
        /// </summary>
        Task<JsonElement> CreateAsync(ProductionFlowDto dto);

        /// <summary>
        /// 🗑️ ID'ye göre ProductionFlow kaydını siler
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 🏭 ProductionFlow'dan FinishedGoodsReceipt (Mamul Fişi) oluşturur
        /// </summary>
        Task<JsonElement> CreateFinishedGoodsReceiptAsync(FinishedGoodsReceiptParamDto param);
    }
}