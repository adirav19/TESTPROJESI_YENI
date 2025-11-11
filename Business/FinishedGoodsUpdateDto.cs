using System.ComponentModel.DataAnnotations;

namespace TESTPROJESI.Business.DTOs
{
    public class FinishedGoodsUpdateDto
    {
        [Required(ErrorMessage = "Fiş numarası zorunludur")]
        public string FisNo { get; set; } = string.Empty;

        public string? Tarih { get; set; }
        public string? Depo { get; set; }
        public string? Malzeme { get; set; }

        [Range(0.001, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır")]
        public decimal? Miktar { get; set; }

        public string? Birim { get; set; }
        public string? Aciklama { get; set; }
    }
}