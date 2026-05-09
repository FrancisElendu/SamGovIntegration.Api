using System.Text.Json.Serialization;

namespace SamGovIntegration.Api.Models
{
    public class ContractAward
    {
        [JsonPropertyName("contractId")]
        public ContractId ContractId { get; set; } = new();

        [JsonPropertyName("coreData")]
        public CoreData CoreData { get; set; } = new();

        [JsonPropertyName("awardDetails")]
        public AwardDetails AwardDetails { get; set; } = new();
    }

    public class ContractId
    {
        [JsonPropertyName("subtier")]
        public Subtier Subtier { get; set; } = new();

        [JsonPropertyName("piid")]
        public string Piid { get; set; } = string.Empty;

        [JsonPropertyName("modificationNumber")]
        public string ModificationNumber { get; set; } = string.Empty;

        [JsonPropertyName("transactionNumber")]
        public string TransactionNumber { get; set; } = string.Empty;

        [JsonPropertyName("referencedIDVSubtier")]
        public ReferencedIdvSubtier ReferencedIDVSubtier { get; set; } = new();

        [JsonPropertyName("referencedIDVPiid")]
        public string ReferencedIDVPiid { get; set; } = string.Empty;

        [JsonPropertyName("referencedIDVModificationNumber")]
        public string ReferencedIDVModificationNumber { get; set; } = string.Empty;

        [JsonPropertyName("reasonForModification")]
        public ReasonForModification ReasonForModification { get; set; } = new();
    }

    public class Subtier
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ReferencedIdvSubtier
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ReasonForModification
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class CoreData
    {
        [JsonPropertyName("coreVersionId")]
        public string CoreVersionId { get; set; } = string.Empty;

        [JsonPropertyName("solicitationId")]
        public string SolicitationId { get; set; } = string.Empty;

        [JsonPropertyName("solicitationDate")]
        public string SolicitationDate { get; set; } = string.Empty;

        [JsonPropertyName("awardOrIDV")]
        public string AwardOrIDV { get; set; } = string.Empty;

        [JsonPropertyName("awardOrIDVType")]
        public AwardOrIDVType AwardOrIDVType { get; set; } = new();

        [JsonPropertyName("federalOrganization")]
        public FederalOrganization FederalOrganization { get; set; } = new();

        [JsonPropertyName("acquisitionData")]
        public AcquisitionData AcquisitionData { get; set; } = new();

        [JsonPropertyName("legislativeMandates")]
        public LegislativeMandates LegislativeMandates { get; set; } = new();

        [JsonPropertyName("principalPlaceOfPerformance")]
        public PrincipalPlaceOfPerformance PrincipalPlaceOfPerformance { get; set; } = new();

        [JsonPropertyName("productOrServiceInformation")]
        public CoreProductOrServiceInformation ProductOrServiceInformation { get; set; } = new();

        [JsonPropertyName("competitionInformation")]
        public CoreCompetitionInformation CompetitionInformation { get; set; } = new();
    }

    public class AwardOrIDVType
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class FederalOrganization
    {
        [JsonPropertyName("contractingInformation")]
        public ContractingInformation ContractingInformation { get; set; } = new();

        [JsonPropertyName("fundingInformation")]
        public FundingInformation FundingInformation { get; set; } = new();
    }

    public class ContractingInformation
    {
        [JsonPropertyName("contractingDepartment")]
        public ContractingDepartment ContractingDepartment { get; set; } = new();

        [JsonPropertyName("contractingSubtier")]
        public ContractingSubtier ContractingSubtier { get; set; } = new();

        [JsonPropertyName("contractingOffice")]
        public ContractingOffice ContractingOffice { get; set; } = new();
    }

    public class ContractingDepartment
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ContractingSubtier
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ContractingOffice
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }

    public class FundingInformation
    {
        [JsonPropertyName("fundingDepartment")]
        public FundingDepartment FundingDepartment { get; set; } = new();

        [JsonPropertyName("fundingSubtier")]
        public FundingSubtier FundingSubtier { get; set; } = new();

        [JsonPropertyName("fundingOffice")]
        public FundingOffice FundingOffice { get; set; } = new();
    }

    public class FundingDepartment
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class FundingSubtier
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class FundingOffice
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class AcquisitionData
    {
        [JsonPropertyName("typeOfContractPricing")]
        public TypeOfContractPricing TypeOfContractPricing { get; set; } = new();

        [JsonPropertyName("multiyearContract")]
        public string MultiyearContract { get; set; } = string.Empty;
    }

    public class TypeOfContractPricing
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class LegislativeMandates
    {
        [JsonPropertyName("clingerCohenAct")]
        public ClingerCohenAct ClingerCohenAct { get; set; } = new();
    }

    public class ClingerCohenAct
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class PrincipalPlaceOfPerformance
    {
        [JsonPropertyName("city")]
        public City City { get; set; } = new();

        [JsonPropertyName("state")]
        public State State { get; set; } = new();

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; } = string.Empty;
    }

    public class City
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class State
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class CoreProductOrServiceInformation
    {
        [JsonPropertyName("productOrService")]
        public ProductOrService ProductOrService { get; set; } = new();

        [JsonPropertyName("principalNaics")]
        public List<PrincipalNaic> PrincipalNaics { get; set; } = new();
    }

    public class ProductOrService
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class PrincipalNaic
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class CoreCompetitionInformation
    {
        [JsonPropertyName("extentCompeted")]
        public ExtentCompeted ExtentCompeted { get; set; } = new();
    }

    public class ExtentCompeted
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class AwardDetails
    {
        [JsonPropertyName("dates")]
        public AwardDates Dates { get; set; } = new();

        [JsonPropertyName("dollars")]
        public AwardDollars Dollars { get; set; } = new();

        [JsonPropertyName("contractData")]
        public ContractData ContractData { get; set; } = new();

        [JsonPropertyName("productOrServiceInformation")]
        public AwardProductOrServiceInformation ProductOrServiceInformation { get; set; } = new();

        [JsonPropertyName("awardeeData")]
        public AwardeeData AwardeeData { get; set; } = new();
    }

    public class AwardDates
    {
        [JsonPropertyName("dateSigned")]
        public string DateSigned { get; set; } = string.Empty;

        [JsonPropertyName("periodOfPerformanceStartDate")]
        public string PeriodOfPerformanceStartDate { get; set; } = string.Empty;

        [JsonPropertyName("currentCompletionDate")]
        public string CurrentCompletionDate { get; set; } = string.Empty;
    }

    public class AwardDollars
    {
        [JsonPropertyName("actionObligation")]
        public decimal ActionObligation { get; set; }
    }

    public class ContractData
    {
        [JsonPropertyName("numberOfActions")]
        public string NumberOfActions { get; set; } = string.Empty;
    }

    public class AwardProductOrServiceInformation
    {
        [JsonPropertyName("descriptionOfContractRequirement")]
        public string DescriptionOfContractRequirement { get; set; } = string.Empty;
    }

    public class AwardeeData
    {
        [JsonPropertyName("awardeeHeader")]
        public AwardeeHeader AwardeeHeader { get; set; } = new();

        [JsonPropertyName("awardeeUEIInformation")]
        public AwardeeUEIInformation AwardeeUEIInformation { get; set; } = new();
    }

    public class AwardeeHeader
    {
        [JsonPropertyName("awardeeName")]
        public string AwardeeName { get; set; } = string.Empty;
    }

    public class AwardeeUEIInformation
    {
        [JsonPropertyName("uniqueEntityId")]
        public string UniqueEntityId { get; set; } = string.Empty;
    }
}
