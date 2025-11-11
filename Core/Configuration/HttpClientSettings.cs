namespace TESTPROJESI.Core.Configuration
{
    /// <summary>
    /// 🌐 HttpClient yapılandırma ayarları
    /// </summary>
    public class HttpClientSettings
    {
        public const string SectionName = "HttpClient";

        public int TimeoutSeconds { get; set; } = 30;
        public int RetryCount { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 2;
        public int CircuitBreakerThreshold { get; set; } = 5;
        public int CircuitBreakerDurationSeconds { get; set; } = 30;

        public TimeSpan Timeout => TimeSpan.FromSeconds(TimeoutSeconds);
        public TimeSpan RetryDelay => TimeSpan.FromSeconds(RetryDelaySeconds);
        public TimeSpan CircuitBreakerDuration => TimeSpan.FromSeconds(CircuitBreakerDurationSeconds);
    }
}
