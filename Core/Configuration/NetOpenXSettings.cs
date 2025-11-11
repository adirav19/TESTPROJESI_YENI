namespace TESTPROJESI.Core.Configuration
{
    /// <summary>
    /// 🔧 NetOpenX API ayarları
    /// </summary>
    public class NetOpenXSettings
    {
        public const string SectionName = "NetOpenX";

        public string BaseUrl { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DbName { get; set; } = string.Empty;
        public string DbUser { get; set; } = string.Empty;
        public string DbPassword { get; set; } = string.Empty;
        public string DbType { get; set; } = string.Empty;

        /// <summary>
        /// Ayarların geçerli olup olmadığını kontrol eder
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(BaseUrl) &&
                   !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(DbName);
        }

        /// <summary>
        /// Ayarları validate eder, geçersizse exception fırlatır
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrEmpty(BaseUrl))
                throw new InvalidOperationException("NetOpenX BaseUrl ayarı yapılandırılmamış!");

            if (string.IsNullOrEmpty(Username))
                throw new InvalidOperationException("NetOpenX Username ayarı yapılandırılmamış!");

            if (string.IsNullOrEmpty(Password))
                throw new InvalidOperationException("NetOpenX Password ayarı yapılandırılmamış!");

            if (string.IsNullOrEmpty(DbName))
                throw new InvalidOperationException("NetOpenX DbName ayarı yapılandırılmamış!");
        }
    }
}
