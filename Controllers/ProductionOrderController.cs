using Microsoft.AspNetCore.Mvc;
using TESTPROJESI.Business.ProductionOrder;
using TESTPROJESI.Services.Interfaces;

namespace TESTPROJESI.Controllers
{
    public class ProductionOrderController : Controller
    {
        private readonly IProductionOrderService _service;

        public ProductionOrderController(IProductionOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductionOrderCreateDto dto)
        {
            var result = await _service.CreateProductionOrderAsync(dto);
            return Json(result);
        }
    }
}
