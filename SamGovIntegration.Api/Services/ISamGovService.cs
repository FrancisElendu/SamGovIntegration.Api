using SamGovIntegration.Api.Models;
using SamGovIntegration.Api.Models.ManualMappings;

namespace SamGovIntegration.Api.Services
{
    public interface ISamGovService
    {
        Task<FetchResult> FetchAndStoreContractsAsync(DateTime startDate, DateTime endDate);
        Task<List<SimplifiedContractAward>> FetchContractsFromApiAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
