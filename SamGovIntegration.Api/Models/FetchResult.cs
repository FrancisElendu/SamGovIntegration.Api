namespace SamGovIntegration.Api.Models
{
    public class FetchResult
    {
        public int TotalFetched { get; set; }
        public int TotalStored { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool IsSuccessful => Errors.Count == 0;
    }
}
