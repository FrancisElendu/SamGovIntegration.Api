using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamGovIntegration.Api.Models
{
    /// <summary>
    /// Database entity for storing contract awards
    /// </summary>
    [Table("ContractAwards")]
    public class ContractAwardEntity
    {
        [Key]
        [MaxLength(50)]
        public string Piid { get; set; }  // Primary key - unique contract ID

        [MaxLength(500)]
        public string DescriptionOfRequirement { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DollarsObligated { get; set; }

        public DateTime ApprovedDate { get; set; }

        [MaxLength(200)]
        public string AwardeeName { get; set; }

        [MaxLength(20)]
        public string AwardeeUei { get; set; }

        [MaxLength(200)]
        public string DepartmentOrAgency { get; set; }

        [MaxLength(10)]
        public string NaicsCode { get; set; }

        [MaxLength(10)]
        public string ProductOrServiceCode { get; set; }

        [MaxLength(50)]
        public string SolicitationId { get; set; }

        // Metadata fields for tracking
        public DateTime DateFetched { get; set; }

        [MaxLength(20)]
        public string FetchBatchId { get; set; }  // Track which batch run this came from

        [MaxLength(100)]
        public string SourceEndpoint { get; set; }  // Which SAM.gov endpoint provided this

        public bool IsActive { get; set; } = true;
    }
}
