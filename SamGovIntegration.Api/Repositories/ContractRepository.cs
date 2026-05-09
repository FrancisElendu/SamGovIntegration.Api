using Microsoft.EntityFrameworkCore;
using SamGovIntegration.Api.Data;
using SamGovIntegration.Api.Models;

namespace SamGovIntegration.Api.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContractRepository> _logger;

        public ContractRepository(ApplicationDbContext context, ILogger<ContractRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<FetchBatchEntity> StartBatchAsync(string batchId, DateTime startDate, DateTime endDate, decimal? minAmount, decimal? maxAmount)
        {
            var batch = new FetchBatchEntity
            {
                BatchId = batchId,
                StartTime = DateTime.UtcNow,
                Status = "Running",
                DateRangeStart = startDate,
                DateRangeEnd = endDate,
                MinDollarAmount = minAmount,
                MaxDollarAmount = maxAmount
            };

            await _context.FetchBatches.AddAsync(batch);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // This will show the actual database error
                var innerException = ex.InnerException?.Message;
                throw new Exception($"Database error: {innerException}", ex);
            }

            return batch;
        }

        public async Task CompleteBatchAsync(string batchId, int recordsFetched, int recordsStored)
        {
            var batch = await _context.FetchBatches.FindAsync(batchId);
            if (batch != null)
            {
                batch.EndTime = DateTime.UtcNow;
                batch.TotalRecordsFetched = recordsFetched;
                batch.TotalRecordsStored = recordsStored;
                batch.Status = "Completed";
                await _context.SaveChangesAsync();
            }
        }

        public async Task FailBatchAsync(string batchId, string errorMessage)
        {
            var batch = await _context.FetchBatches.FindAsync(batchId);
            if (batch != null)
            {
                batch.EndTime = DateTime.UtcNow;
                batch.Status = "Failed";
                batch.ErrorMessage = errorMessage;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ContractAwardEntity> GetContractByPiidAsync(string piid)
        {
            return await _context.ContractAwards.FindAsync(piid);
        }

        public async Task AddOrUpdateContractAsync(ContractAwardEntity contract)
        {
            var existing = await GetContractByPiidAsync(contract.Piid);

            if (existing == null)
            {
                await _context.ContractAwards.AddAsync(contract);
            }
            else
            {
                // Update existing contract with latest data
                _context.Entry(existing).CurrentValues.SetValues(contract);
                existing.DateFetched = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddContractsBulkAsync(IEnumerable<ContractAwardEntity> contracts)
        {
            //var existingPiids = await _context.ContractAwards
            //    .Where(c => contractList.Select(x => x.Piid).Contains(c.Piid))
            //    .Select(c => c.Piid)
            //    .ToHashSetAsync();

            //because .ToHashSetAsync() is not supported in EF Core 6, we need to materialize the list first and then create a HashSet from it.
            // For bulk operations, use ExecuteUpdate or EF Core 7+ bulk operations

            var contractList = contracts.ToList();
            var existingPiids = await _context.ContractAwards
                .Where(c => contractList.Select(x => x.Piid).Contains(c.Piid))
                .Select(c => c.Piid)
                .ToListAsync();

            var existingPiidsSet = existingPiids.ToHashSet();  // Then convert to HashSet

            var toAdd = contractList.Where(c => !existingPiids.Contains(c.Piid)).ToList();
            var toUpdate = contractList.Where(c => existingPiids.Contains(c.Piid)).ToList();

            if (toAdd.Any())
            {
                await _context.ContractAwards.AddRangeAsync(toAdd);
            }

            foreach (var contract in toUpdate)
            {
                var existing = await _context.ContractAwards.FindAsync(contract.Piid);
                _context.Entry(existing).CurrentValues.SetValues(contract);
                existing.DateFetched = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ContractExistsAsync(string piid)
        {
            return await _context.ContractAwards.AnyAsync(c => c.Piid == piid);
        }

        public async Task<int> GetContractCountByBatchAsync(string batchId)
        {
            return await _context.ContractAwards.CountAsync(c => c.FetchBatchId == batchId);
        }
    }
}
