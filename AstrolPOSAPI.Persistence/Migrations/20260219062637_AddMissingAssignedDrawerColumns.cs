using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrolPOSAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingAssignedDrawerColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultShortcutBar",
                table: "AssignedDrawers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SessionTimeIn",
                table: "AssignedDrawers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SessionTimeOut",
                table: "AssignedDrawers",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultShortcutBar",
                table: "AssignedDrawers");

            migrationBuilder.DropColumn(
                name: "SessionTimeIn",
                table: "AssignedDrawers");

            migrationBuilder.DropColumn(
                name: "SessionTimeOut",
                table: "AssignedDrawers");
        }
    }
}
