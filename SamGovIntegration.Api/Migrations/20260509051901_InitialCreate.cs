using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamGovIntegration.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractAwards",
                columns: table => new
                {
                    Piid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionOfRequirement = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DollarsObligated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AwardeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AwardeeUei = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DepartmentOrAgency = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NaicsCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProductOrServiceCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SolicitationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateFetched = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FetchBatchId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SourceEndpoint = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAwards", x => x.Piid);
                });

            migrationBuilder.CreateTable(
                name: "FetchBatches",
                columns: table => new
                {
                    BatchId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalRecordsFetched = table.Column<int>(type: "int", nullable: false),
                    TotalRecordsStored = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DateRangeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRangeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinDollarAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxDollarAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FetchBatches", x => x.BatchId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractAwards_ApprovedDate",
                table: "ContractAwards",
                column: "ApprovedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAwards_ApprovedDate_DollarsObligated",
                table: "ContractAwards",
                columns: new[] { "ApprovedDate", "DollarsObligated" });

            migrationBuilder.CreateIndex(
                name: "IX_ContractAwards_AwardeeUei",
                table: "ContractAwards",
                column: "AwardeeUei");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAwards_NaicsCode",
                table: "ContractAwards",
                column: "NaicsCode");

            migrationBuilder.CreateIndex(
                name: "IX_FetchBatches_Status",
                table: "FetchBatches",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractAwards");

            migrationBuilder.DropTable(
                name: "FetchBatches");
        }
    }
}
