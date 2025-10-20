using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.FinancialFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFinancialFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "demo");

            migrationBuilder.CreateSequence(
                name: "financialflowseq",
                schema: "demo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "FinancialFlows",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FlowType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Subsidiairy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialFlows", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialFlows",
                schema: "demo");

            migrationBuilder.DropSequence(
                name: "financialflowseq",
                schema: "demo");
        }
    }
}
