using System.Text.Json;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Core.Extensions;
using TESTPROJESI.Core.Mapping;

namespace TESTPROJESI.Business.Mappers
{
    /// <summary>
    /// 📦 FinishedGoods Mapper
    /// JsonElement ↔ DTO dönüşümleri
    /// </summary>
    public class FinishedGoodsMapper : BaseMapper<JsonElement, FinishedGoodsCreateDto>
    {
        public override FinishedGoodsCreateDto Map(JsonElement source)
        {
            return new FinishedGoodsCreateDto
            {
                FisNo = source.GetStringSafe("UretSon_FisNo"),
                Tarih = source.GetStringSafe("UretSon_Tarih"),
                Depo = source.GetStringSafe("UretSon_Depo"),
                Malzeme = source.GetStringSafe("UretSon_Mamul"),
                Miktar = source.GetDecimalSafe("UretSon_Miktar"),
                Birim = "Adet" // Default value veya source'dan alınabilir
            };
        }

        public override JsonElement MapBack(FinishedGoodsCreateDto destination)
        {
            throw new NotImplementedException("FinishedGoods için JsonElement'e dönüş gerekmez");
        }

        /// <summary>
        /// JsonElement'ten DetailDto'ya özel mapping
        /// </summary>
        public FinishedGoodsDetailDto MapToDetail(JsonElement source)
        {
            var dto = new FinishedGoodsDetailDto
            {
                UretSon_FisNo = source.GetStringSafe("UretSon_FisNo"),
                UretSon_Tarih = source.GetStringSafe("UretSon_Tarih"),
                UretSon_SipNo = source.GetStringSafe("UretSon_SipNo"),
                UretSon_Mamul = source.GetStringSafe("UretSon_Mamul"),
                UretSon_Miktar = source.GetDecimalSafe("UretSon_Miktar"),
                UretSon_Depo = source.GetIntSafe("UretSon_Depo"),
                Aciklama = source.GetStringSafe("Aciklama"),
                KayitYapanKul = source.GetStringSafe("KayitYapanKul"),
                Kalem = MapKalemList(source)
            };

            return dto;
        }

        /// <summary>
        /// Kalem listesi mapping
        /// </summary>
        private List<KalemDto> MapKalemList(JsonElement source)
        {
            var kalemList = new List<KalemDto>();

            if (!source.TryGetProperty("Kalem", out var kalemArray) ||
                kalemArray.ValueKind != JsonValueKind.Array)
                return kalemList;

            foreach (var item in kalemArray.EnumerateArray())
            {
                kalemList.Add(new KalemDto
                {
                    FisNo = source.GetStringSafe("UretSon_FisNo"),
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

            return kalemList;
        }
    }
}
