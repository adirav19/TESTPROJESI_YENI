using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Services.Interfaces;
using TESTPROJESI.Models;

namespace TESTPROJESI.Controllers
{
    public class FinishedGoodsController : Controller
    {
        private readonly IFinishedGoodsService _service;
        private readonly ILogger<FinishedGoodsController> _logger;

        public FinishedGoodsController(
            IFinishedGoodsService service,
            ILogger<FinishedGoodsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// 🏠 Ana sayfa
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 📋 Tüm fişleri listele
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Json(ApiResponse<List<FinishedGoodsCreateDto>>.SuccessResponse(
                    result,
                    $"{result.Count} adet fiş listelendi"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş listesi alınırken hata");
                return StatusCode(500, ApiResponse<List<FinishedGoodsCreateDto>>.ErrorResponse(
                    "Fiş listesi alınamadı",
                    ex.Message
                ));
            }
        }

        /// <summary>
        /// 🔍 Fiş detayı
        /// </summary>
        [HttpGet("FinishedGoods/Detail/{fisNo}")]
        public async Task<IActionResult> Detail(string fisNo)
        {
            try
            {
                var result = await _service.GetByIdAsync(fisNo);

                if (result == null)
                {
                    return NotFound(ApiResponse<FinishedGoodsDetailDto>.ErrorResponse(
                        "Fiş bulunamadı",
                        $"{fisNo} numaralı fiş sistemde mevcut değil"
                    ));
                }

                return Json(ApiResponse<FinishedGoodsDetailDto>.SuccessResponse(
                    result,
                    "Fiş detayı getirildi"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş detayı alınırken hata: {FisNo}", fisNo);
                return StatusCode(500, ApiResponse<FinishedGoodsDetailDto>.ErrorResponse(
                    "Fiş detayı alınamadı",
                    ex.Message
                ));
            }
        }

        /// <summary>
        /// ➕ Yeni fiş oluştur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FinishedGoodsCreateDto dto)
        {
            // ✅ Model validation
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Geçersiz veri gönderildi",
                    errors
                ));
            }

            try
            {
                var result = await _service.CreateAsync(dto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş oluşturulurken hata: {FisNo}", dto.FisNo);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Fiş oluşturulamadı",
                    ex.Message
                ));
            }
        }

        /// <summary>
        /// ✏️ Fiş güncelle
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FinishedGoodsUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Geçersiz veri gönderildi",
                    errors
                ));
            }

            try
            {
                var result = await _service.UpdateAsync(dto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş güncellenirken hata: {FisNo}", dto.FisNo);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Fiş güncellenemedi",
                    ex.Message
                ));
            }
        }

        /// <summary>
        /// 🗑️ Fiş sil
        /// </summary>
        [HttpDelete("FinishedGoods/Delete/{fisNo}")]
        public async Task<IActionResult> Delete(string fisNo)
        {
            try
            {
                var result = await _service.DeleteAsync(fisNo);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş silinirken hata: {FisNo}", fisNo);
                return StatusCode(500, ApiResponse<bool>.ErrorResponse(
                    "Fiş silinemedi",
                    ex.Message
                ));
            }
        }

        /// <summary>
        /// 🔢 Kalem miktarı güncelle
        /// </summary>
        [HttpPost("FinishedGoods/UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] KalemDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.StokKodu))
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Geçersiz veri",
                    "Stok kodu boş olamaz"
                ));
            }

            try
            {
                var result = await _service.UpdateQuantityAsync(dto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Miktar güncellenirken hata: {StokKodu}", dto.StokKodu);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Miktar güncellenemedi",
                    ex.Message
                ));
            }
        }

        /// <summary>
        /// ✏️ Inline güncelleme (satır içi düzenleme)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateInline([FromBody] dynamic dto)
        {
            try
            {
                // DTO'yu deserialize et
                string fisNo = dto.FisNo?.ToString() ?? "";
                string field = dto.Field?.ToString() ?? "";
                string value = dto.Value?.ToString() ?? "";

                if (string.IsNullOrEmpty(fisNo) || string.IsNullOrEmpty(field))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Geçersiz veri",
                        "Fiş numarası ve alan adı zorunludur"
                    ));
                }

                // Field'a göre uygun DTO oluştur
                var updateDto = new FinishedGoodsUpdateDto { FisNo = fisNo };

                switch (field.ToLower())
                {
                    case "tarih":
                        updateDto.Tarih = value;
                        break;
                    case "depo":
                        updateDto.Depo = value;
                        break;
                    case "malzeme":
                        updateDto.Malzeme = value;
                        break;
                    case "miktar":
                        if (decimal.TryParse(value, out var miktar))
                            updateDto.Miktar = miktar;
                        break;
                    case "birim":
                        updateDto.Birim = value;
                        break;
                    default:
                        return BadRequest(ApiResponse<object>.ErrorResponse(
                            "Geçersiz alan",
                            $"{field} alanı güncellenemez"
                        ));
                }

                var result = await _service.UpdateAsync(updateDto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Inline güncelleme hatası");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Güncelleme başarısız",
                    ex.Message
                ));
            }
        }
    }
}