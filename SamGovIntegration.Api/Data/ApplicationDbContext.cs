using Microsoft.EntityFrameworkCore;
using SamGovIntegration.Api.Models;

namespace SamGovIntegration.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<ContractAwardEntity> ContractAwards { get; set; }
        public DbSet<FetchBatchEntity> FetchBatches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add indexes for common search patterns
            modelBuilder.Entity<ContractAwardEntity>()
                .HasIndex(e => e.ApprovedDate);

            modelBuilder.Entity<ContractAwardEntity>()
                .HasIndex(e => e.AwardeeUei);

            modelBuilder.Entity<ContractAwardEntity>()
                .HasIndex(e => e.NaicsCode);

            modelBuilder.Entity<ContractAwardEntity>()
                .HasIndex(e => new { e.ApprovedDate, e.DollarsObligated });

            modelBuilder.Entity<FetchBatchEntity>()
                .HasIndex(e => e.Status);
        }
    }
}
