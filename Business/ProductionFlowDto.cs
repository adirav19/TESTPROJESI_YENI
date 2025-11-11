namespace TESTPROJESI.Business.DTOs
{
    /// <summary>
    /// 🏭 ProductionFlow (UAK) için temel DTO
    /// </summary>
    public class ProductionFlowDto
    {
        public string IsEmriNo { get; set; }           // İş Emri No
        public string CONFSIRANO { get; set; }         // Konfigürasyon Sıra No
        public int IncKeyNo { get; set; }              // Inc Key No
        public string StokKodu { get; set; }           // Stok Kodu
        public string OpKodu { get; set; }             // Operasyon Kodu
        public string OPSIRANO { get; set; }           // Operasyon Sıra No
        public string IstasyonKodu { get; set; }       // İstasyon Kodu
        public decimal SIMULTANEOPR { get; set; }      // Eşzamanlı Operasyon
        public int MRPMAKINENO { get; set; }           // Makine No
        public int MRPISCINO { get; set; }             // İşçi No
        public string BASLANGICTARIH { get; set; }     // Başlangıç Tarihi (YYYY-MM-DD HH:mm:ss)
        public int BASLANGICVARDIYA { get; set; }      // Başlangıç Vardiyası
        public decimal SURE { get; set; }              // Süre
        public int SURETIPI { get; set; }              // Süre Tipi
        public string BITISTARIHSAAT { get; set; }     // Bitiş Tarihi Saati
        public string AKTIVITEKODU { get; set; }       // Aktivite Kodu
        public string ARIZAKODU { get; set; }          // Arıza Kodu
        public bool ISLENDI { get; set; }              // İşlendi mi?
        public decimal URETILENMIKTAR { get; set; }    // Üretilen Miktar
        public decimal FIREMIKTAR { get; set; }        // Fire Miktar
        public string ProjeKodu { get; set; }          // Proje Kodu
        public string RevNo { get; set; }              // Revizyon No
        public string SERILOTNO { get; set; }          // Seri/Lot No
        public int USKDEPOKODU { get; set; }           // Depo Kodu
        public string ACIK1 { get; set; }              // Açık Alan 1
        public string ACIK2 { get; set; }              // Açık Alan 2
        public string ACIK3 { get; set; }              // Açık Alan 3
        public string ACIK4 { get; set; }              // Açık Alan 4
        public string STOKURS_INCKEYNO { get; set; }   // Stok Ürün Inc Key No
        public string YapKod { get; set; }             // Yapı Kodu
        public string OPR_KAYNAK_VIEW { get; set; }    // Operasyon Kaynak View
        public bool BASLADI_BITMEDI { get; set; }      // Başladı Bitmedi
        public string SUBISEMRI_NO { get; set; }       // Alt İş Emri No
        public int UAKKaynakListCount { get; set; }    // UAK Kaynak Listesi Sayısı
        public decimal OLCUBRMIKTAR { get; set; }      // Ölçü Birimi Miktar
        public decimal OLCUBRFIRE { get; set; }        // Ölçü Birimi Fire

        // Alt listeler (opsiyonel)
        public List<ShrinkageDetailDto> ShrinkageDetailList { get; set; } = new();
        public List<UAKKaynakDto> UAKKaynakLists { get; set; } = new();
    }

    /// <summary>
    /// 🔥 Fire (Shrinkage) detay bilgisi
    /// </summary>
    public class ShrinkageDetailDto
    {
        public string FireKodu { get; set; }
        public decimal Miktar { get; set; }
        public string FireAck { get; set; }
    }

    /// <summary>
    /// 🔧 UAK Kaynak listesi
    /// </summary>
    public class UAKKaynakDto
    {
        public int MRPISCI_ID { get; set; }
        public decimal Miktar { get; set; }
        public int GozSayisi { get; set; }
        public decimal Omur { get; set; }
    }

    /// <summary>
    /// 📋 FinishedGoodsReceipt parametresi (UAK'tan Mamul Fişi oluşturmak için)
    /// </summary>
    public class FinishedGoodsReceiptParamDto
    {
        public string IsEmriNoAralikAlt { get; set; }
        public string IsEmriNoAralikUst { get; set; }
        public string TarihAralikAlt { get; set; }
        public string TarihAralikUst { get; set; }
        public int VardiyaAralikAlt { get; set; }
        public int VardiyaAralikUst { get; set; }
        public bool FislerIsEmriBazindaTekTekOlusturulsun { get; set; }
        public string FisNoSerisi { get; set; }
        public string KayitTarihi { get; set; }
        public int Oncelik { get; set; }
        public int DepoOnceligi { get; set; }
        public int GirisDepo { get; set; }
        public int CikisDepo { get; set; }
        public bool FislerOtomatikUretilsin { get; set; }
        public bool OtoYariMamullerdeGirdiCikti { get; set; }
        public bool OtoYariMamullerdeStoktanKullan { get; set; }
        public int MamullerOlcuBirimi { get; set; }
        public int Bakiye { get; set; }
        public int HataliKayitlarPolitikasi { get; set; }
    }
}