using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.StaffService.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "StaffMembers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "StaffMembers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_Department",
                table: "StaffMembers",
                column: "Department");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_Email",
                table: "StaffMembers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_IsActive",
                table: "StaffMembers",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StaffMembers_Department",
                table: "StaffMembers");

            migrationBuilder.DropIndex(
                name: "IX_StaffMembers_Email",
                table: "StaffMembers");

            migrationBuilder.DropIndex(
                name: "IX_StaffMembers_IsActive",
                table: "StaffMembers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "StaffMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "StaffMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
