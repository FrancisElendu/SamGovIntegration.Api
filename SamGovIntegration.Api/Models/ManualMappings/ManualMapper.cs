namespace SamGovIntegration.Api.Models.ManualMappings
{
    public static class ManualMapper
    {
        public static ContractAwardEntity MapToEntity(SimplifiedContractAward award, string batchId)
        {
            return new ContractAwardEntity
            {
                Piid = award.Piid,
                DescriptionOfRequirement = award.DescriptionOfRequirement,
                DollarsObligated = award.DollarsObligated,
                ApprovedDate = DateTime.TryParse(award.ApprovedDate ?? string.Empty, out var approvedDate) ? approvedDate : DateTime.MinValue,
                AwardeeName = award.AwardeeName ?? string.Empty,
                AwardeeUei = award.AwardeeUei ?? string.Empty,
                DepartmentOrAgency = award.DepartmentOrAgency ?? string.Empty,
                NaicsCode = award.NaicsCode ?? string.Empty,
                ProductOrServiceCode = award.ProductOrServiceCode ?? string.Empty,
                SolicitationId = award.SolicitationId ?? string.Empty,
                DateFetched = DateTime.UtcNow,
                FetchBatchId = batchId,
                SourceEndpoint = "contract-awards/v1/search",
                IsActive = true
            };
        }
    }
}

