using System.Text.Json;
using TESTPROJESI.Business.DTOs;
using TESTPROJESI.Core.Extensions;
using TESTPROJESI.Core.Mapping;

namespace TESTPROJESI.Business.Mappers
{
    /// <summary>
    /// 🏭 ProductionFlow Mapper
    /// JsonElement ↔ ProductionFlowDto dönüşümleri
    /// </summary>
    public class ProductionFlowMapper : BaseMapper<JsonElement, ProductionFlowDto>
    {
        public override ProductionFlowDto Map(JsonElement source)
        {
            return new ProductionFlowDto
            {
                IsEmriNo = source.GetStringSafe("IsEmriNo"),
                CONFSIRANO = source.GetStringSafe("CONFSIRANO"),
                IncKeyNo = source.GetIntSafe("IncKeyNo"),
                StokKodu = source.GetStringSafe("StokKodu"),
                OpKodu = source.GetStringSafe("OpKodu"),
                OPSIRANO = source.GetStringSafe("OPSIRANO"),
                IstasyonKodu = source.GetStringSafe("IstasyonKodu"),
                SIMULTANEOPR = source.GetDecimalSafe("SIMULTANEOPR"),
                MRPMAKINENO = source.GetIntSafe("MRPMAKINENO"),
                MRPISCINO = source.GetIntSafe("MRPISCINO"),
                BASLANGICTARIH = source.GetStringSafe("BASLANGICTARIH"),
                BASLANGICVARDIYA = source.GetIntSafe("BASLANGICVARDIYA"),
                SURE = source.GetDecimalSafe("SURE"),
                SURETIPI = source.GetIntSafe("SURETIPI"),
                BITISTARIHSAAT = source.GetStringSafe("BITISTARIHSAAT"),
                AKTIVITEKODU = source.GetStringSafe("AKTIVITEKODU"),
                ARIZAKODU = source.GetStringSafe("ARIZAKODU"),
                ISLENDI = source.GetBoolSafe("ISLENDI"),
                URETILENMIKTAR = source.GetDecimalSafe("URETILENMIKTAR"),
                FIREMIKTAR = source.GetDecimalSafe("FIREMIKTAR"),
                ProjeKodu = source.GetStringSafe("ProjeKodu"),
                RevNo = source.GetStringSafe("RevNo"),
                SERILOTNO = source.GetStringSafe("SERILOTNO"),
                USKDEPOKODU = source.GetIntSafe("USKDEPOKODU"),
                ACIK1 = source.GetStringSafe("ACIK1"),
                ACIK2 = source.GetStringSafe("ACIK2"),
                ACIK3 = source.GetStringSafe("ACIK3"),
                ACIK4 = source.GetStringSafe("ACIK4"),
                STOKURS_INCKEYNO = source.GetStringSafe("STOKURS_INCKEYNO"),
                YapKod = source.GetStringSafe("YapKod"),
                OPR_KAYNAK_VIEW = source.GetStringSafe("OPR_KAYNAK_VIEW"),
                BASLADI_BITMEDI = source.GetBoolSafe("BASLADI_BITMEDI"),
                SUBISEMRI_NO = source.GetStringSafe("SUBISEMRI_NO"),
                UAKKaynakListCount = source.GetIntSafe("UAKKaynakListCount"),
                OLCUBRMIKTAR = source.GetDecimalSafe("OLCUBRMIKTAR"),
                OLCUBRFIRE = source.GetDecimalSafe("OLCUBRFIRE")
            };
        }

        public override JsonElement MapBack(ProductionFlowDto destination)
        {
            throw new NotImplementedException("ProductionFlow için JsonElement'e dönüş gerekmez");
        }
    }
}
