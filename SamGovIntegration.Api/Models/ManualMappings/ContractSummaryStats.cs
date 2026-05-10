namespace SamGovIntegration.Api.Models.ManualMappings
{
    public class ContractSummaryStats
    {
        public int TotalContracts { get; set; }
        public decimal TotalObligatedAmount { get; set; }
        public decimal AverageContractAmount { get; set; }
        public decimal MinContractAmount { get; set; }
        public decimal MaxContractAmount { get; set; }
        public int UniqueAwardees { get; set; }
        public Dictionary<string, int> TopAgencies { get; set; } = new();
        public Dictionary<string, int> TopNaicsCodes { get; set; } = new();
    }
}
