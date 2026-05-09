using System.Text.Json.Serialization;

namespace SamGovIntegration.Api.Models
{
    public class SamGovApiResponse
    {
        [JsonPropertyName("awardSummary")]
        public List<ContractAward> AwardSummary { get; set; } = new();

        [JsonPropertyName("totalRecords")]
        public string TotalRecords { get; set; } = "0";

        [JsonPropertyName("limit")]
        public string Limit { get; set; } = "100";

        [JsonPropertyName("offset")]
        public string Offset { get; set; } = "0";
    }
}