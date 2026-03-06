using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.BedService.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ward",
                table: "Beds",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BedNumber",
                table: "Beds",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_BedNumber",
                table: "Beds",
                column: "BedNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Beds_Ward",
                table: "Beds",
                column: "Ward");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beds_BedNumber",
                table: "Beds");

            migrationBuilder.DropIndex(
                name: "IX_Beds_Ward",
                table: "Beds");

            migrationBuilder.AlterColumn<string>(
                name: "Ward",
                table: "Beds",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BedNumber",
                table: "Beds",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
