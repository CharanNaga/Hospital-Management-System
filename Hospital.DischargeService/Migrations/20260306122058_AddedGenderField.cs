using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.DischargeService.Migrations
{
    /// <inheritdoc />
    public partial class AddedGenderField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdmittedOn",
                table: "DischargeSummaries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientGender",
                table: "DischargeSummaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmittedOn",
                table: "DischargeSummaries");

            migrationBuilder.DropColumn(
                name: "PatientGender",
                table: "DischargeSummaries");
        }
    }
}
