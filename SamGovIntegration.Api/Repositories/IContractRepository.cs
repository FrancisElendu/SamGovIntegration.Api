using SamGovIntegration.Api.Models;

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
    }
}
