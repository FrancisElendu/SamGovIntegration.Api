namespace SamGovIntegration.Api.Models.ManualMappings
{
    public class SimplifiedContractAward
    {
        public string Piid { get; set; } = string.Empty;
        public string DescriptionOfRequirement { get; set; } = string.Empty;
        public decimal DollarsObligated { get; set; }
        public string ApprovedDate { get; set; } = string.Empty;
        public string AwardeeName { get; set; } = string.Empty;
        public string AwardeeUei { get; set; } = string.Empty;
        public string DepartmentOrAgency { get; set; } = string.Empty;
        public string NaicsCode { get; set; } = string.Empty;
        public string ProductOrServiceCode { get; set; } = string.Empty;
        public string SolicitationId { get; set; } = string.Empty;
    }
}
