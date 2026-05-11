using Microsoft.EntityFrameworkCore;
using SamGovIntegration.Api.Data;
using SamGovIntegration.Api.Models;
using SamGovIntegration.Api.Models.ManualMappings;

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
            var contractList = contracts.ToList();

            if (!contractList.Any())
                return;

            // Clean Piid values (trim whitespace)
            foreach (var contract in contractList)
            {
                contract.Piid = contract.Piid?.Trim();
            }

            // Get existing Piids
            var existingContractPiids = await _context.ContractAwards
                .Select(c => c.Piid)
                .ToListAsync();

            var existingPiidsSet = new HashSet<string>(existingContractPiids, StringComparer.OrdinalIgnoreCase);

            var toAdd = new List<ContractAwardEntity>();
            var toUpdate = new List<ContractAwardEntity>();

            foreach (var contract in contractList)
            {
                if (existingPiidsSet.Contains(contract.Piid))
                {
                    toUpdate.Add(contract);
                }
                else
                {
                    toAdd.Add(contract);
                }
            }

            // Handle new contracts
            if (toAdd.Any())
            {
                await _context.ContractAwards.AddRangeAsync(toAdd);
            }

            // Handle existing contracts
            if (toUpdate.Any())
            {
                foreach (var contract in toUpdate)
                {
                    contract.DateFetched = DateTime.UtcNow;
                }
                _context.ContractAwards.UpdateRange(toUpdate);
            }

            await _context.SaveChangesAsync();

            ////because .ToHashSetAsync() is not supported in EF Core 6, we need to materialize the list first and then create a HashSet from it.
            //// For bulk operations, use ExecuteUpdate or EF Core 7+ bulk operations

            //var contractList = contracts.ToList();

            //if (!contractList.Any())
            //    return;

            //// Clean Piid values (trim whitespace)
            //foreach (var contract in contractList)
            //{
            //    contract.Piid = contract.Piid?.Trim();
            //}

            //var existingPiids = await _context.ContractAwards
            //    .Where(c => contractList.Select(x => x.Piid).Contains(c.Piid))
            //    .Select(c => c.Piid)
            //    .ToListAsync();

            //var existingPiidsSet = existingPiids.ToHashSet();  // Then convert to HashSet

            //var toAdd = contractList.Where(c => !existingPiids.Contains(c.Piid)).ToList();
            //var toUpdate = contractList.Where(c => existingPiids.Contains(c.Piid)).ToList();

            //if (toAdd.Any())
            //{
            //    await _context.ContractAwards.AddRangeAsync(toAdd);
            //}

            //if (toUpdate.Any()) 
            //{
            //    // Update DateFetched before updating
            //    foreach (var contract in toUpdate)
            //    {
            //        contract.DateFetched = DateTime.UtcNow;
            //    }

            //    // UpdateRange will attach and mark all as modified
            //    _context.ContractAwards.UpdateRange(toUpdate);

            //    //foreach (var contract in toUpdate)
            //    //{

            //    //    // Attach the entity and mark it as modified
            //    //    _context.Entry(contract).State = EntityState.Modified;

            //    //    // Ensure DateFetched is updated
            //    //    contract.DateFetched = DateTime.UtcNow;


            //    //    //var existing = await _context.ContractAwards.FindAsync(contract.Piid);
            //    //    //_context.Entry(existing).CurrentValues.SetValues(contract);
            //    //    //existing.DateFetched = DateTime.UtcNow;
            //    //}
            //}

            //await _context.SaveChangesAsync();
        }

        public async Task<bool> ContractExistsAsync(string piid)
        {
            return await _context.ContractAwards.AnyAsync(c => c.Piid == piid);
        }

        public async Task<int> GetContractCountByBatchAsync(string batchId)
        {
            return await _context.ContractAwards.CountAsync(c => c.FetchBatchId == batchId);
        }

        public async Task<(IEnumerable<ContractAwardEntity> Contracts, int TotalCount)> GetStoredContractsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        int page = 1,
        int pageSize = 50)
        {
            // Validate pagination parameters
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 50 : (pageSize > 100 ? 100 : pageSize);

            // Start building the query
            var query = _context.ContractAwards.AsQueryable();

            // Apply date filters (using ApprovedDate)
            if (startDate.HasValue)
            {
                query = query.Where(c => c.ApprovedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.ApprovedDate <= endDate.Value);
            }

            // Apply dollar amount filters
            if (minAmount.HasValue)
            {
                query = query.Where(c => c.DollarsObligated >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(c => c.DollarsObligated <= maxAmount.Value);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination and ordering (most recent first)
            var contracts = await query
                .OrderByDescending(c => c.ApprovedDate)
                .ThenBy(c => c.Piid)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (contracts, totalCount);
        }

        public async Task<ContractSummaryStats> GetContractSummaryStatsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _context.ContractAwards.AsQueryable();

            // Apply date filters
            if (startDate.HasValue)
            {
                query = query.Where(c => c.ApprovedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.ApprovedDate <= endDate.Value);
            }

            // Calculate statistics
            var stats = new ContractSummaryStats();

            if (await query.AnyAsync())
            {
                stats.TotalContracts = await query.CountAsync();
                stats.TotalObligatedAmount = await query.SumAsync(c => c.DollarsObligated);
                stats.AverageContractAmount = await query.AverageAsync(c => c.DollarsObligated);
                stats.MinContractAmount = await query.MinAsync(c => c.DollarsObligated);
                stats.MaxContractAmount = await query.MaxAsync(c => c.DollarsObligated);
                stats.UniqueAwardees = await query.Select(c => c.AwardeeUei).Distinct().CountAsync();

                // Get top 5 agencies by contract count
                stats.TopAgencies = await query
                    .Where(c => c.DepartmentOrAgency != null)
                    .GroupBy(c => c.DepartmentOrAgency)
                    .Select(g => new { Agency = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToDictionaryAsync(g => g.Agency, g => g.Count);

                // Get top 5 NAICS codes by contract count
                stats.TopNaicsCodes = await query
                    .Where(c => c.NaicsCode != null)
                    .GroupBy(c => c.NaicsCode)
                    .Select(g => new { Naics = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToDictionaryAsync(g => g.Naics, g => g.Count);
            }

            return stats;
        }
    }
}
