using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.DischargeService.Migrations
{
    /// <inheritdoc />
    public partial class AddedDoctorNameToDischargeSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DischargingDoctorName",
                table: "DischargeSummaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DischargingDoctorName",
                table: "DischargeSummaries");
        }
    }
}
