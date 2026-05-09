namespace SamGovIntegration.Api.Options
{
    public class SamApiOptions
    {
        public const string SectionName = "SamApi";
        public string BaseUrl { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 120;
        public int MaxRetryAttempts { get; set; } = 5;
        public int CircuitBreakerEventsBeforeBreaking { get; set; } = 3;
        public int CircuitBreakerDurationMinutes { get; set; } = 1;
    }
}
