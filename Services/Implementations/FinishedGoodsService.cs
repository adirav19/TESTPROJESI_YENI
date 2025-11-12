using System.Text.Json;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Business.Mappers;
using TESTPROJESI.Services.Interfaces;
using TESTPROJESI.Services.Base;
using TESTPROJESI.Models;
using TESTPROJESI.Core.Extensions;
using TESTPROJESI.Core.Constants;
using Microsoft.Extensions.Logging;

namespace TESTPROJESI.Services.Implementations
{
    /// <summary>
    /// 📦 FinishedGoods Service - Refactored with Generic Base
    /// ✅ Mapper Pattern kullanır
    /// ✅ RequestBuilder ile esnek query oluşturur
    /// ✅ Generic base service'ten kalıtım alır
    /// </summary>
    public class FinishedGoodsService : GenericModuleService<FinishedGoodsCreateDto>, IFinishedGoodsService
    {
        private readonly FinishedGoodsMapper _detailMapper;

        public FinishedGoodsService(
            IBaseApiService apiService,
            ITokenManager tokenManager,
            ILogger<FinishedGoodsService> logger)
            : base(
                  apiService,
                  tokenManager,
                  logger,
                  new FinishedGoodsMapper(),
                  AppConstants.Endpoints.FinishedGoods,
                  new ModuleServiceOptions
                  {
                      DefaultSortField = "UretSon_FisNo",
                      DefaultSortDescending = true
                  })
        {
            _detailMapper = new FinishedGoodsMapper();
        }

        /// <summary>
        /// 🔍 Fiş detayı (mapper kullanır)
        /// </summary>
        public async Task<FinishedGoodsDetailDto?> GetByIdAsync(string fisNo)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var responseJson = await _apiService.GetAsync<JsonElement>($"{_endpoint}/{fisNo}", token);
                var data = responseJson.UnwrapData();

                if (data.ValueKind != JsonValueKind.Object)
                {
                    _logger.LogWarning("⚠️ Fiş bulunamadı: {FisNo}", fisNo);
                    return null;
                }

                // ✅ Mapper kullan
                var dto = _detailMapper.MapToDetail(data);
                _logger.LogInformation("✅ Fiş detayı getirildi: {FisNo}", fisNo);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş detayı hatası: {FisNo}", fisNo);
                return null;
            }
        }

        /// <summary>
        /// ➕ Yeni fiş oluştur (2 aşamalı: ReceiptProduce + Save)
        /// </summary>
        public new async Task<ApiResponse<JsonElement>> CreateAsync(FinishedGoodsCreateDto dto)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();

                var payload = new
                {
                    UretSon_FisNo = dto.FisNo,
                    UretSon_Tarih = dto.Tarih,
                    UretSon_Depo = int.Parse(dto.Depo),
                    UretSon_Mamul = dto.Malzeme,
                    UretSon_Miktar = dto.Miktar,
                    Mamul_Olcu_Birimi = 0,
                    Aciklama = "Web arayüzünden oluşturuldu",
                    TransactSupport = true,
                    MuhasebelesmisBelge = true
                };

                // 1️⃣ Adım: Fişi hazırla
                _logger.LogInformation("📝 1. Adım: Fiş hazırlanıyor: {FisNo}", dto.FisNo);
                await _apiService.PostAsync<JsonElement>($"{_endpoint}/ReceiptProduce", payload, token);

                // 2️⃣ Adım: Fişi kaydet
                _logger.LogInformation("💾 2. Adım: Fiş kaydediliyor: {FisNo}", dto.FisNo);
                var saveResponse = await _apiService.PostAsync<JsonElement>($"{_endpoint}/Save", new { }, token);

                return ApiResponse<JsonElement>.SuccessResponse(
                    saveResponse,
                    string.Format(AppConstants.SuccessMessages.Created, $"{dto.FisNo} numaralı fiş")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş oluşturma hatası: {FisNo}", dto.FisNo);
                return ApiResponse<JsonElement>.ErrorResponse(
                    string.Format(AppConstants.ErrorMessages.CreateFailed, "Fiş"),
                    ex.Message
                );
            }
        }

        /// <summary>
        /// ✏️ Fiş güncelle (partial update desteği)
        /// </summary>
        public async Task<ApiResponse<JsonElement>> UpdateAsync(FinishedGoodsUpdateDto dto)
        {
            try
            {
                // Mevcut fişi getir
                var current = await GetByIdAsync(dto.FisNo);
                if (current == null)
                {
                    return ApiResponse<JsonElement>.ErrorResponse(
                        string.Format(AppConstants.ErrorMessages.NotFound, "Fiş"),
                        $"{dto.FisNo} numaralı fiş sistemde mevcut değil"
                    );
                }

                var token = await _tokenManager.GetTokenAsync();

                // Sadece değişen alanları güncelle (partial update)
                var payload = new
                {
                    UretSon_FisNo = dto.FisNo,
                    UretSon_Tarih = dto.Tarih ?? current.UretSon_Tarih,
                    UretSon_Depo = dto.Depo ?? current.UretSon_Depo.ToString(),
                    UretSon_Mamul = dto.Malzeme ?? current.UretSon_Mamul,
                    UretSon_Miktar = dto.Miktar ?? current.UretSon_Miktar,
                    Aciklama = dto.Aciklama ?? current.Aciklama,
                    Kalem = current.Kalem,
                    SubelerdeOrtak = true,
                    IsletmelerdeOrtak = true,
                    TransactSupport = true
                };

                var response = await _apiService.PutAsync<JsonElement>($"{_endpoint}/{dto.FisNo}", payload, token);

                _logger.LogInformation("✅ Fiş güncellendi: {FisNo}", dto.FisNo);
                return ApiResponse<JsonElement>.SuccessResponse(
                    response,
                    string.Format(AppConstants.SuccessMessages.Updated, $"{dto.FisNo} numaralı fiş")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Fiş güncelleme hatası: {FisNo}", dto.FisNo);
                return ApiResponse<JsonElement>.ErrorResponse(
                    string.Format(AppConstants.ErrorMessages.UpdateFailed, "Fiş"),
                    ex.Message
                );
            }
        }

        /// <summary>
        /// 🗑️ Fiş sil (base metodunu override etmiyoruz, kullanıyoruz)
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteAsync(string fisNo)
        {
            return await base.DeleteAsync(fisNo);
        }

        /// <summary>
        /// 🔢 Kalem miktarını güncelle
        /// </summary>
        public async Task<ApiResponse<JsonElement>> UpdateQuantityAsync(KalemDto dto)
        {
            try
            {
                if (dto.FisNo.IsNullOrWhiteSpace())
                {
                    return ApiResponse<JsonElement>.ErrorResponse(
                        "Geçersiz istek",
                        "Fiş numarası belirtilmedi"
                    );
                }

                var current = await GetByIdAsync(dto.FisNo);
                if (current == null)
                {
                    return ApiResponse<JsonElement>.ErrorResponse(
                        string.Format(AppConstants.ErrorMessages.NotFound, "Fiş"),
                        $"{dto.FisNo} numaralı fiş sistemde mevcut değil"
                    );
                }

                // Kalemi bul ve güncelle
                var updatedKalem = current.Kalem.FirstOrDefault(x => x.StokKodu == dto.StokKodu);
                if (updatedKalem == null)
                {
                    return ApiResponse<JsonElement>.ErrorResponse(
                        "Kalem bulunamadı",
                        $"{dto.StokKodu} stok kodu fişte bulunamadı"
                    );
                }

                updatedKalem.Miktar = dto.Miktar;

                var token = await _tokenManager.GetTokenAsync();

                var payload = new
                {
                    current.UretSon_FisNo,
                    current.UretSon_Tarih,
                    UretSon_Depo = current.UretSon_Depo.ToString(),
                    current.UretSon_Mamul,
                    current.UretSon_Miktar,
                    current.Aciklama,
                    current.KayitYapanKul,
                    Kalem = current.Kalem
                };

                var response = await _apiService.PostAsync<JsonElement>($"{_endpoint}/Save", payload, token);

                _logger.LogInformation("✅ Miktar güncellendi - Fiş: {FisNo}, Stok: {StokKodu}, Miktar: {Miktar}",
                    dto.FisNo, dto.StokKodu, dto.Miktar);

                return ApiResponse<JsonElement>.SuccessResponse(
                    response,
                    $"✅ {dto.StokKodu} kaleminin miktarı {dto.Miktar} olarak güncellendi"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Miktar güncelleme hatası: {StokKodu}", dto.StokKodu);
                return ApiResponse<JsonElement>.ErrorResponse(
                    "Miktar güncellenemedi",
                    ex.Message
                );
            }
        }
    }
}
