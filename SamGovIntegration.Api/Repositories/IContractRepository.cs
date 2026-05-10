using SamGovIntegration.Api.Models;
using SamGovIntegration.Api.Models.ManualMappings;

namespace SamGovIntegration.Api.Repositories
{
    public interface IContractRepository
    {
        Task<FetchBatchEntity> StartBatchAsync(string batchId, DateTime startDate, DateTime endDate, decimal? minAmount, decimal? maxAmount);
        Task CompleteBatchAsync(string batchId, int recordsFetched, int recordsStored);
        Task FailBatchAsync(string batchId, string errorMessage);
        Task<ContractAwardEntity> GetContractByPiidAsync(string piid);
        Task AddOrUpdateContractAsync(ContractAwardEntity contract);
        Task AddContractsBulkAsync(IEnumerable<ContractAwardEntity> contracts);
        Task<bool> ContractExistsAsync(string piid);
        Task<int> GetContractCountByBatchAsync(string batchId);
        Task<(IEnumerable<ContractAwardEntity> Contracts, int TotalCount)> GetStoredContractsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        int page = 1,
        int pageSize = 50);

        Task<ContractSummaryStats> GetContractSummaryStatsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}
