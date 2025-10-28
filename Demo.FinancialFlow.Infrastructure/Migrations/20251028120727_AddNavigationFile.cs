using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.FinancialFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StorageFileId",
                schema: "demo",
                table: "FinancialFlows",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FinancialFlowFileAudits_StorageFileId",
                schema: "demo",
                table: "FinancialFlowFileAudits",
                column: "StorageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialFlows_StorageFileId",
                schema: "demo",
                table: "FinancialFlows",
                column: "StorageFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialFlows_FinancialFlowFileAudits_StorageFileId",
                schema: "demo",
                table: "FinancialFlows",
                column: "StorageFileId",
                principalSchema: "demo",
                principalTable: "FinancialFlowFileAudits",
                principalColumn: "StorageFileId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialFlows_FinancialFlowFileAudits_StorageFileId",
                schema: "demo",
                table: "FinancialFlows");

            migrationBuilder.DropIndex(
                name: "IX_FinancialFlows_StorageFileId",
                schema: "demo",
                table: "FinancialFlows");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FinancialFlowFileAudits_StorageFileId",
                schema: "demo",
                table: "FinancialFlowFileAudits");

            migrationBuilder.DropColumn(
                name: "StorageFileId",
                schema: "demo",
                table: "FinancialFlows");
        }
    }
}
