using Microsoft.AspNetCore.Mvc;
using SamGovIntegration.Api.Repositories;
using SamGovIntegration.Api.Services;

namespace SamGovIntegration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly ISamGovService _samGovService;
        private readonly ILogger<ContractsController> _logger;
        private readonly IContractRepository _contractRepository;

        public ContractsController(ISamGovService samGovService, ILogger<ContractsController> logger, IContractRepository contractRepository)
        {
            _samGovService = samGovService;
            _logger = logger;
            _contractRepository = contractRepository;
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
            try
            {
                // Validate date range
                if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
                {
                    return BadRequest("Start date must be before end date");
                }

                // Validate pagination
                if (page < 1)
                {
                    return BadRequest("Page number must be 1 or greater");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100");
                }

                // Validate amount range
                if (minAmount.HasValue && minAmount.Value < 0)
                {
                    return BadRequest("Minimum amount cannot be negative");
                }

                if (maxAmount.HasValue && maxAmount.Value < 0)
                {
                    return BadRequest("Maximum amount cannot be negative");
                }

                if (minAmount.HasValue && maxAmount.HasValue && minAmount.Value > maxAmount.Value)
                {
                    return BadRequest("Minimum amount cannot be greater than maximum amount");
                }

                // Query the database
                var (contracts, totalCount) = await _contractRepository.GetStoredContractsAsync(
                    startDate, endDate, minAmount, maxAmount, page, pageSize);

                // Return the results with pagination metadata
                return Ok(new
                {
                    data = contracts,
                    pagination = new
                    {
                        currentPage = page,
                        pageSize = pageSize,
                        totalCount = totalCount,
                        totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                        hasPreviousPage = page > 1,
                        hasNextPage = page < (int)Math.Ceiling((double)totalCount / pageSize)
                    },
                    filters = new
                    {
                        startDate,
                        endDate,
                        minAmount,
                        maxAmount
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stored contracts");
                return StatusCode(500, new { error = "Failed to retrieve contracts", details = ex.Message });
            }
        }

        /// <summary>
        /// Get summary statistics for stored contracts
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetContractStats(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var stats = await _contractRepository.GetContractSummaryStatsAsync(startDate, endDate);

                return Ok(new
                {
                    statistics = stats,
                    filters = new
                    {
                        startDate,
                        endDate
                    },
                    generatedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contract statistics");
                return StatusCode(500, new { error = "Failed to retrieve statistics", details = ex.Message });
            }
        }


        /// <summary>
        /// Get a single contract by its PIID
        /// </summary>
        [HttpGet("{piid}")]
        public async Task<IActionResult> GetContractByPiid(string piid)
        {
            if (string.IsNullOrWhiteSpace(piid))
            {
                return BadRequest("PIID cannot be empty");
            }

            try
            {
                var contract = await _contractRepository.GetContractByPiidAsync(piid);

                if (contract == null)
                {
                    return NotFound(new { message = $"Contract with PIID '{piid}' not found" });
                }

                return Ok(contract);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contract {Piid}", piid);
                return StatusCode(500, new { error = "Failed to retrieve contract", details = ex.Message });
            }
        }

    }
}
