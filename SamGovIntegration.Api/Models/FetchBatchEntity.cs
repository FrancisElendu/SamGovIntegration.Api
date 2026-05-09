using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamGovIntegration.Api.Models
{
    /// <summary>
    /// Tracks each execution of the contract fetch process
    /// </summary>
    [Table("FetchBatches")]
    public class FetchBatchEntity
    {
        [Key]
        [MaxLength(20)]
        public string BatchId { get; set; }  // Format: YYYYMMDD_HHMMSS

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int TotalRecordsFetched { get; set; }

        public int TotalRecordsStored { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }  // Running, Completed, Failed

        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        public DateTime DateRangeStart { get; set; }

        public DateTime DateRangeEnd { get; set; }

        public decimal? MinDollarAmount { get; set; }

        public decimal? MaxDollarAmount { get; set; }
    }
}
