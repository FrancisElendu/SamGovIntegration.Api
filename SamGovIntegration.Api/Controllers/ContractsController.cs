using Microsoft.AspNetCore.Mvc;
using SamGovIntegration.Api.Services;

namespace SamGovIntegration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly ISamGovService _samGovService;
        private readonly ILogger<ContractsController> _logger;

        public ContractsController(ISamGovService samGovService, ILogger<ContractsController> logger)
        {
            _samGovService = samGovService;
            _logger = logger;
        }

        /// <summary>
        /// Fetch contracts for a date range and store in database
        /// </summary>
        [HttpPost("fetch")]
        public async Task<IActionResult> FetchContracts([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date");
            }

            // Validate date range is not too large (SAM.gov may have limits)
            if ((endDate - startDate).TotalDays > 90)
            {
                return BadRequest("Date range cannot exceed 90 days. Please make multiple requests.");
            }

            try
            {
                var result = await _samGovService.FetchAndStoreContractsAsync(startDate, endDate);

                return Ok(new
                {
                    message = $"Successfully processed {result.TotalStored} contracts",
                    batchId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"),
                    totalFetched = result.TotalFetched,
                    totalStored = result.TotalStored
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching contracts");
                return StatusCode(500, new { error = "Failed to fetch contracts", details = ex.Message });
            }
        }

        /// <summary>
        /// Get stored contracts from database with filters
        /// </summary>
        [HttpGet("stored")]
        public async Task<IActionResult> GetStoredContracts(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] decimal? minAmount,
            [FromQuery] decimal? maxAmount,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            //TO DO:
            // Implementation would query the database using the repository
            // This is a placeholder - you'd add a query method to IContractRepository

            return Ok(new { message = "Query endpoint - implement repository query methods" });
        }
    }
}
