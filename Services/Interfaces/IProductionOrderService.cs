using TESTPROJESI.Business.ProductionOrder;
using TESTPROJESI.Models;

namespace TESTPROJESI.Services.Interfaces
{
    public interface IProductionOrderService
    {
        Task<ApiResponse<string>> CreateProductionOrderAsync(ProductionOrderCreateDto dto);
    }
}
