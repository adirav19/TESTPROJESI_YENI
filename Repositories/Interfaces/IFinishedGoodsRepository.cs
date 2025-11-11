using System.Text.Json;
using System.Threading.Tasks;
using TESTPROJESI.Business.DTOs;

namespace TESTPROJESI.Repositories.Interfaces
{
    /// <summary>
    /// 📦 FinishedGoods modülüne özel repository
    /// </summary>
    public interface IFinishedGoodsRepository : IGenericRepository<FinishedGoodsCreateDto>
    {
        // 🔹 Özel metodlar
        Task<FinishedGoodsDetailDto?> GetDetailByIdAsync(string fisNo);
        Task<JsonElement> UpdateQuantityAsync(KalemDto dto);
    }
}
