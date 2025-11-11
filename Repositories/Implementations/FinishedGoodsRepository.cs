using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Repositories.Interfaces;
using TESTPROJESI.Services.Interfaces;
using TESTPROJESI.Core.Extensions; // ✅ Extension metodlar

namespace TESTPROJESI.Repositories.Implementations
{
    /// <summary>
    /// 📦 FinishedGoods repository - Extension metodları kullanır
    /// </summary>
    public class FinishedGoodsRepository : GenericRepository<FinishedGoodsCreateDto>, IFinishedGoodsRepository
    {
        public FinishedGoodsRepository(
            IBaseApiService apiService,
            ITokenManager tokenManager,
            ILogger<GenericRepository<FinishedGoodsCreateDto>> logger)
            : base(apiService, tokenManager, logger, "FinishedGoodsReceiptWChanges")
        {
        }

        // 🔹 Detaylı fiş bilgisi getir (Kalem listesi ile)
        public async Task<FinishedGoodsDetailDto?> GetDetailByIdAsync(string fisNo)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var url = $"{_endpoint}/{fisNo}";

                var responseJson = await _apiService.GetAsync<JsonElement>(url, token);

                if (responseJson.ValueKind != JsonValueKind.Object ||
                    !responseJson.TryGetProperty("Data", out var data))
                {
                    _logger.LogWarning("⚠️ Beklenmeyen JSON formatı: {Json}", responseJson.ToString());
                    return null;
                }

                // ✅ Extension metodları kullan
                var dto = new FinishedGoodsDetailDto
                {
                    UretSon_FisNo = data.GetStringSafe("UretSon_FisNo"),
                    UretSon_Tarih = data.GetStringSafe("UretSon_Tarih"),
                    UretSon_SipNo = data.GetStringSafe("UretSon_SipNo"),
                    UretSon_Mamul = data.GetStringSafe("UretSon_Mamul"),
                    UretSon_Miktar = data.GetDecimalSafe("UretSon_Miktar"),
                    UretSon_Depo = data.GetIntSafe("UretSon_Depo"),
                    Aciklama = data.GetStringSafe("Aciklama"),
                    KayitYapanKul = data.GetStringSafe("KayitYapanKul"),
                    Kalem = new List<KalemDto>()
                };

                if (data.TryGetProperty("Kalem", out var kalemArray) && kalemArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in kalemArray.EnumerateArray())
                    {
                        dto.Kalem.Add(new KalemDto
                        {
                            Index = item.GetIntSafe("Index"),
                            IncKeyNo = item.GetIntSafe("IncKeyNo"),
                            StokKodu = item.GetStringSafe("StokKodu"),
                            DepoKodu = item.GetIntSafe("DepoKodu"),
                            Miktar = (double)item.GetDecimalSafe("Miktar"),
                            Aciklama = item.GetStringSafe("Aciklama"),
                            SeriVarMi = item.GetBoolSafe("SeriVarMi"),
                            BGTIP = item.GetStringSafe("BGTIP")
                        });
                    }
                }

                return dto;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "❌ GetDetailById hatası: {FisNo}", fisNo);
                throw;
            }
        }

        // 🔹 Kalem miktarını güncelle
        public async Task<JsonElement> UpdateQuantityAsync(KalemDto dto)
        {
            try
            {
                var token = await _tokenManager.GetTokenAsync();
                var fisNo = dto.FisNo;

                if (string.IsNullOrWhiteSpace(fisNo))
                {
                    string json = @"{""success"":false,""message"":""Fiş numarası belirtilmedi!""}";
                    return JsonSerializer.Deserialize<JsonElement>(json);
                }

                var current = await GetDetailByIdAsync(fisNo);

                if (current == null)
                {
                    string json = @"{""success"":false,""message"":""Fiş bulunamadı!""}";
                    return JsonSerializer.Deserialize<JsonElement>(json);
                }

                // Kalemi güncelle
                var updatedKalem = current.Kalem.FirstOrDefault(x => x.StokKodu == dto.StokKodu);
                if (updatedKalem != null)
                    updatedKalem.Miktar = dto.Miktar;

                var payload = new
                {
                    current.UretSon_FisNo,
                    current.UretSon_Tarih,
                    current.UretSon_Depo,
                    current.UretSon_Mamul,
                    current.UretSon_Miktar,
                    current.Aciklama,
                    current.KayitYapanKul,
                    Kalem = current.Kalem
                };

                var response = await _apiService.PostAsync<JsonElement>(
                    $"{_endpoint}/Save",
                    payload,
                    token
                );

                _logger.LogInformation("✅ {StokKodu} miktarı {Miktar} olarak güncellendi",
                    dto.StokKodu, dto.Miktar);

                return response;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "❌ UpdateQuantity hatası: {StokKodu}", dto.StokKodu);
                string json = @"{""success"":false,""message"":""Sunucu hatası!""}";
                return JsonSerializer.Deserialize<JsonElement>(json);
            }
        }

        // ❌ KALDIRILDI: Helper metodlar (artık JsonExtensions kullanılıyor)
    }
}