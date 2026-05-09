using SamGovIntegration.Api.Models;
using SamGovIntegration.Api.Models.ManualMappings;
using SamGovIntegration.Api.Repositories;
using System.Text.Json;

namespace SamGovIntegration.Api.Services
{
    public class SamGovService : ISamGovService
    {
        private readonly HttpClient _httpClient;
        private readonly IContractRepository _repository;
        private readonly ILogger<SamGovService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public SamGovService(
        HttpClient httpClient,
        IConfiguration configuration,
        IContractRepository repository,
        ILogger<SamGovService> logger)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
            _apiKey = configuration["SamApi:Key"] ?? throw new InvalidOperationException("SAM API key not configured");
            _baseUrl = configuration["SamApi:BaseUrl"] ?? throw new InvalidOperationException("SAM base url not configured");
        }

        /// <summary>
        /// Main entry point: Fetches contracts from SAM.gov and stores them in the database
        /// </summary>
        public async Task<FetchResult> FetchAndStoreContractsAsync(DateTime startDate, DateTime endDate)
        {
            var batchId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var fetchResult = new FetchResult();

            _logger.LogInformation("Starting contract fetch batch {BatchId} for date range {StartDate} to {EndDate}",
                batchId, startDate, endDate);

            // Start batch tracking
            await _repository.StartBatchAsync(batchId, startDate, endDate, 10_000_000m, 200_000_000m);

            try
            {
                // Fetch contracts with retry handling (Polly handles retries automatically)
                var contracts = await FetchContractsFromApiAsync(startDate, endDate);
                fetchResult.TotalFetched = contracts.Count;

                _logger.LogInformation("Fetched {Count} contracts from SAM.gov API", contracts.Count);

                // Transform to database entities
                var entities = contracts.Select(c => ManualMapper.MapToEntity(c, batchId)).ToList();

                // Store in database with upsert logic
                await _repository.AddContractsBulkAsync(entities);
                fetchResult.TotalStored = entities.Count;

                // Complete batch tracking
                await _repository.CompleteBatchAsync(batchId, fetchResult.TotalFetched, fetchResult.TotalStored);

                _logger.LogInformation("Successfully stored {Stored} contracts from batch {BatchId}",
                    fetchResult.TotalStored, batchId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch and store contracts for batch {BatchId}", batchId);
                await _repository.FailBatchAsync(batchId, ex.Message);
                throw;
            }

            return fetchResult;
        }


        /// <summary>
        /// Core API fetching logic with pagination handling
        /// </summary>
        public async Task<List<SimplifiedContractAward>> FetchContractsFromApiAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var allContracts = new List<SimplifiedContractAward>();
            int pageOffset = 0;
            const int pageSize = 100; // Max per page according to SAM.gov API

            bool hasMorePages = true;

            while (hasMorePages && !cancellationToken.IsCancellationRequested)
            {
                // Build request URL with proper encoding
                var requestUrl = BuildRequestUrl(startDate, endDate, pageSize, pageOffset);

                _logger.LogDebug("Fetching page {PageNumber} from SAM.gov API", pageOffset / pageSize + 1);

                var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

                // Check for rate limiting in response headers[citation:2]
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogWarning("Rate limit hit. Retry policy should handle this.");
                    // Polly's retry policy will handle this automatically
                }

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                var apiResponse = JsonSerializer.Deserialize<SamGovApiResponse>(jsonResponse);

                if (apiResponse?.AwardSummary != null && apiResponse.AwardSummary.Any())
                {
                    // Map to SimplifiedContractAward and filter
                    var simplifiedContracts = apiResponse.AwardSummary
                        .Select(award => award.ToSimplified())
                        .Where(c => c.DollarsObligated >= 10_000_000m && c.DollarsObligated <= 200_000_000m)
                        .ToList();

                    allContracts.AddRange(simplifiedContracts);

                    // Check if there are more pages
                    var totalPages = (int)Math.Ceiling((double)Convert.ToInt32(apiResponse.TotalRecords) / pageSize);
                    var currentPage = pageOffset / pageSize;
                    hasMorePages = currentPage + 1 < totalPages;

                    if (hasMorePages)
                    {
                        pageOffset += pageSize;

                        // Add delay between requests to avoid rate limiting[citation:2]
                        await Task.Delay(500, cancellationToken);
                    }
                }
                else
                {
                    hasMorePages = false;
                }
            }

            return allContracts;
        }

        private string BuildRequestUrl(DateTime startDate, DateTime endDate, int pageSize, int pageOffset)
        {
            // Note: Replace "prod/.../search" with the actual endpoint path from SAM.gov documentation
            // The correct format should be documented at open.gsa.gov/api

            var startDateStr = startDate.ToString("MM/dd/yyyy");
            var endDateStr = endDate.ToString("MM/dd/yyyy");

            // URL encode bracket characters for the dollar range
            var dollarRange = $"%5B10000000,200000000%5D";  // [10000000,200000000]
            var dateRange = $"%5B{startDateStr},{endDateStr}%5D"; // [date1,date2]

            //return $"{_baseUrl}contract-awards/v1/search?api_key={_apiKey}&dollarsObligated={dollarRange}&approvedDate={dateRange}&limit={pageSize}&offset={pageOffset}";

            return $"{_baseUrl}contract-awards/v1/search?api_key={_apiKey}&dollarsObligated={dollarRange}&dateSigned={dateRange}&limit={pageSize}&offset={pageOffset}";
        }
    }
}
