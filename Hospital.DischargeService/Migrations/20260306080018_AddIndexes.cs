using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.DischargeService.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DischargeSummaries_DischargedOn",
                table: "DischargeSummaries",
                column: "DischargedOn");

            migrationBuilder.CreateIndex(
                name: "IX_DischargeSummaries_PatientId",
                table: "DischargeSummaries",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DischargeSummaries_DischargedOn",
                table: "DischargeSummaries");

            migrationBuilder.DropIndex(
                name: "IX_DischargeSummaries_PatientId",
                table: "DischargeSummaries");
        }
    }
}
