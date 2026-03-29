using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrolPOSAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumnsAndConsolidateNoSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultFontSize",
                table: "TouchScreens");

            migrationBuilder.DropColumn(
                name: "ColumnSpan",
                table: "TouchScreenButtons");

            migrationBuilder.DropColumn(
                name: "FontSize",
                table: "TouchScreenButtons");

            migrationBuilder.DropColumn(
                name: "IsDefaultImage",
                table: "TouchScreenButtons");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "TouchScreenButtons");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "RequestPath",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "DefaultShortcutBar",
                table: "AssignedDrawers");

            migrationBuilder.DropColumn(
                name: "SessionTimeIn",
                table: "AssignedDrawers");

            migrationBuilder.DropColumn(
                name: "SessionTimeOut",
                table: "AssignedDrawers");

            migrationBuilder.DropColumn(
                name: "LastLoginIP",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "GridRows",
                table: "TouchScreens",
                newName: "Rows");

            migrationBuilder.RenameColumn(
                name: "GridColumns",
                table: "TouchScreens",
                newName: "Columns");

            migrationBuilder.RenameColumn(
                name: "ShowImage",
                table: "TouchScreenButtons",
                newName: "IsVerified");

            migrationBuilder.RenameColumn(
                name: "RowSpan",
                table: "TouchScreenButtons",
                newName: "VerificationAttempts");

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "TouchScreenButtons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanCreate",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanUpdate",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Manual",
                table: "NoSeries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Drawers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DrawerGroups",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "KeyValues",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "AssignedDrawers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTypes_Code",
                table: "StoreTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Code",
                table: "Stores",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Code",
                table: "Companies",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StoreTypes_Code",
                table: "StoreTypes");

            migrationBuilder.DropIndex(
                name: "IX_Stores_Code",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Code",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "TouchScreenButtons");

            migrationBuilder.DropColumn(
                name: "CanCreate",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "CanUpdate",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Manual",
                table: "NoSeries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DrawerGroups");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "AssignedDrawers");

            migrationBuilder.RenameColumn(
                name: "Rows",
                table: "TouchScreens",
                newName: "GridRows");

            migrationBuilder.RenameColumn(
                name: "Columns",
                table: "TouchScreens",
                newName: "GridColumns");

            migrationBuilder.RenameColumn(
                name: "VerificationAttempts",
                table: "TouchScreenButtons",
                newName: "RowSpan");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "TouchScreenButtons",
                newName: "ShowImage");

            migrationBuilder.AddColumn<int>(
                name: "DefaultFontSize",
                table: "TouchScreens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColumnSpan",
                table: "TouchScreenButtons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FontSize",
                table: "TouchScreenButtons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultImage",
                table: "TouchScreenButtons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "TouchScreenButtons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KeyValues",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestPath",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIP",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
