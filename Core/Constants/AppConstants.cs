namespace TESTPROJESI.Core.Constants
{
    /// <summary>
    /// 📌 Uygulama genelinde kullanılan sabit değerler
    /// </summary>
    public static class AppConstants
    {
        public static class Endpoints
        {
            public const string Token = "token";
            public const string FinishedGoods = "FinishedGoodsReceiptWChanges";
            public const string ProductionFlow = "ProductionFlow";
            public const string ARPs = "ARPs";
            public const string ProductionOrder = "v2/ProductionOrder";
        }

        public static class CacheKeys
        {
            public const string AccessToken = "NetOpenXAccessToken";
            public const string TokenExpireTime = "NetOpenXExpireTime";
        }

        public static class DateFormats
        {
            public const string ApiFormat = "yyyy-MM-dd HH:mm:ss";
            public const string DateOnly = "yyyy-MM-dd";
            public const string DisplayFormat = "dd/MM/yyyy";
            public const string DisplayFormatWithTime = "dd/MM/yyyy HH:mm";
        }

        public static class Headers
        {
            public const string Authorization = "Authorization";
            public const string Bearer = "Bearer";
            public const string ContentType = "Content-Type";
            public const string ApplicationJson = "application/json";
        }

        public static class ValidationMessages
        {
            public const string Required = "{0} alanı zorunludur";
            public const string MaxLength = "{0} alanı en fazla {1} karakter olabilir";
            public const string MinLength = "{0} alanı en az {1} karakter olmalıdır";
            public const string Range = "{0} alanı {1} ile {2} arasında olmalıdır";
            public const string Email = "Geçerli bir e-posta adresi giriniz";
            public const string Phone = "Geçerli bir telefon numarası giriniz";
        }

        public static class SuccessMessages
        {
            public const string Created = "{0} başarıyla oluşturuldu";
            public const string Updated = "{0} başarıyla güncellendi";
            public const string Deleted = "{0} başarıyla silindi";
            public const string Listed = "{0} adet kayıt listelendi";
        }

        public static class ErrorMessages
        {
            public const string NotFound = "{0} bulunamadı";
            public const string AlreadyExists = "{0} zaten mevcut";
            public const string CreateFailed = "{0} oluşturulamadı";
            public const string UpdateFailed = "{0} güncellenemedi";
            public const string DeleteFailed = "{0} silinemedi";
            public const string InvalidData = "Geçersiz veri gönderildi";
            public const string ServerError = "Sunucu hatası oluştu";
            public const string Unauthorized = "Yetkiniz bulunmamaktadır";
            public const string TokenExpired = "Token süresi dolmuş";
        }

        public static class Timeouts
        {
            public const int Default = 30;
            public const int Long = 60;
            public const int Short = 10;
        }

        public static class Pagination
        {
            public const int DefaultPageSize = 20;
            public const int MaxPageSize = 100;
            public const int DefaultPage = 1;
        }

        public static class FileSizeLimits
        {
            public const int Image = 5 * 1024 * 1024; // 5 MB
            public const int Document = 10 * 1024 * 1024; // 10 MB
            public const int Excel = 20 * 1024 * 1024; // 20 MB
        }
    }
}
