namespace TESTPROJESI.Business.ProductionOrder
{
    public class ProductionOrderCreateDto
    {
        public bool TransactSupport { get; set; } = true;
        public bool MuhasebelesmisBelge { get; set; } = true;
        public string IsEmriNo { get; set; } = string.Empty;
        public DateTime Tarih { get; set; }
        public string StokKodu { get; set; } = string.Empty;
        public double Miktar { get; set; }
        public int Olcubr { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public DateTime TeslimTarihi { get; set; }
        public string SiparisNo { get; set; } = string.Empty;
        public bool Kapali { get; set; }
        public string Yedek1 { get; set; } = string.Empty;
        public string Yedek2 { get; set; } = string.Empty;
        public string Yedek3 { get; set; } = string.Empty;
        public string Yedek4 { get; set; } = string.Empty;
        public double Yedek5 { get; set; }
        public string ProjeKodu { get; set; } = string.Empty;
        public string RevNo { get; set; } = string.Empty;
        public int Oncelik { get; set; }
        public string RefIsEmriNo { get; set; } = string.Empty;
        public string YapKod { get; set; } = string.Empty;
        public int SipKont { get; set; }
        public string TepeMam { get; set; } = string.Empty;
        public string TepeYapKod { get; set; } = string.Empty;
        public DateTime TepeTarih { get; set; }
        public DateTime CalismaZamani { get; set; }
        public string TepeSipNo { get; set; } = string.Empty;
        public int TepeSipKont { get; set; }
        public string OnayTipi { get; set; } = string.Empty;
        public int OnayNum { get; set; }
        public int SubeKodu { get; set; }
        public int DepoKodu { get; set; }
        public int CikisDepoKodu { get; set; }
        public string SeriNo { get; set; } = string.Empty;
        public bool Rework { get; set; }
        public int UskDurumu { get; set; }
        public int RezervasyonDurumu { get; set; }
        public string SeriNo2 { get; set; } = string.Empty;
        public string HatKodu { get; set; } = string.Empty;
        public bool ReceteSaklansin { get; set; }
    }
}
