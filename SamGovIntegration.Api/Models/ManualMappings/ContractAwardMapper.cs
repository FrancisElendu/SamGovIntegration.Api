namespace SamGovIntegration.Api.Models.ManualMappings
{
    // Extension method to map from the full response
    public static class ContractAwardMapper
    {
        public static SimplifiedContractAward ToSimplified(this ContractAward award)
        {
            return new SimplifiedContractAward
            {
                Piid = award.ContractId?.Piid ?? string.Empty,
                DescriptionOfRequirement = award.AwardDetails?.ProductOrServiceInformation?.DescriptionOfContractRequirement ?? string.Empty,
                DollarsObligated = award.AwardDetails?.Dollars?.ActionObligation ?? 0,
                ApprovedDate = award.AwardDetails?.Dates?.DateSigned ?? string.Empty,
                AwardeeName = award.AwardDetails?.AwardeeData?.AwardeeHeader?.AwardeeName ?? string.Empty,
                AwardeeUei = award.AwardDetails?.AwardeeData?.AwardeeUEIInformation?.UniqueEntityId ?? string.Empty,
                DepartmentOrAgency = award.CoreData?.FederalOrganization?.ContractingInformation?.ContractingDepartment?.Name ?? string.Empty,
                NaicsCode = award.CoreData?.ProductOrServiceInformation?.PrincipalNaics?.FirstOrDefault()?.Code ?? string.Empty,
                ProductOrServiceCode = award.CoreData?.ProductOrServiceInformation?.ProductOrService?.Code ?? string.Empty,
                SolicitationId = award.CoreData?.SolicitationId ?? string.Empty
            };
        }
    }
}
