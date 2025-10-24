using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.FinancialFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFinancialFlowFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialFlowFileAudits",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinancialFlowFileAudits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialFlowFileAudits", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialFlowFileAudits",
                schema: "demo");
        }
    }
}
