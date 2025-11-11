using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Services.Interfaces;

namespace TESTPROJESI.Controllers
{
    public class ProductionFlowController : Controller
    {
        private readonly IProductionFlowService _productionFlowService;
        private readonly ILogger<ProductionFlowController> _logger;

        public ProductionFlowController(
            IProductionFlowService productionFlowService,
            ILogger<ProductionFlowController> logger)
        {
            _productionFlowService = productionFlowService;
            _logger = logger;
        }

        // 🔹 Ana sayfa - Liste görüntüleme
        public async Task<IActionResult> Index()
        {
            try
            {
                var list = await _productionFlowService.GetAllAsync();
                return View(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ProductionFlow listesi alınırken hata: {Message}", ex.Message);
                ViewBag.Hata = ex.Message;
                return View();
            }
        }

        // 🔹 Yeni ProductionFlow oluşturma (AJAX)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductionFlowDto dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "ProductionFlow bilgisi alınamadı." });

            try
            {
                var result = await _productionFlowService.CreateAsync(dto);
                return Ok(new
                {
                    success = true,
                    message = "✅ ProductionFlow kaydı başarıyla oluşturuldu.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ProductionFlow oluşturma hatası: {Message}", ex.Message);
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Hata: {ex.Message}"
                });
            }
        }

        // 🔹 ID'ye göre detay getirme (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _productionFlowService.GetByIdAsync(id);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ProductionFlow detay hatası: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // 🔹 Silme işlemi (AJAX)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productionFlowService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = $"🗑️ {id} ID'li kayıt silindi."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ProductionFlow silme hatası: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // 🔹 UAK'tan Mamul Fişi oluşturma (AJAX)
        [HttpPost]
        public async Task<IActionResult> CreateFinishedGoodsReceipt([FromBody] FinishedGoodsReceiptParamDto param)
        {
            if (param == null)
                return BadRequest(new { success = false, message = "Parametre bilgisi alınamadı." });

            try
            {
                var result = await _productionFlowService.CreateFinishedGoodsReceiptAsync(param);
                return Ok(new
                {
                    success = true,
                    message = "✅ Mamul fişi başarıyla oluşturuldu.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Mamul fişi oluşturma hatası: {Message}", ex.Message);
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Hata: {ex.Message}"
                });
            }
        }
    }
}