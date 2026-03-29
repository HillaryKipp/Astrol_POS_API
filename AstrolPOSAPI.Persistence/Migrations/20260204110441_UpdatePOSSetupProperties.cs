using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrolPOSAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePOSSetupProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TouchScreens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Terminals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Drawers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DefaultScreens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "TouchScreens");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Terminals");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DefaultScreens");
        }
    }
}
