using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrolPOSAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePOSFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Terminals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreOfOperationId",
                table: "Terminals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TerminalId",
                table: "Drawers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "DrawerGroupId",
                table: "Drawers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultScreenId",
                table: "Drawers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Drawers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreOfOperationId",
                table: "Drawers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "DrawerGroups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreOfOperationId",
                table: "DrawerGroups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "DefaultScreens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreOfOperationId",
                table: "DefaultScreens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignedDrawers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DrawerId",
                table: "AssignedDrawers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultScreenId",
                table: "AssignedDrawers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "AssignedDrawers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreOfOperationId",
                table: "AssignedDrawers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_CompanyId",
                table: "Terminals",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_StoreOfOperationId",
                table: "Terminals",
                column: "StoreOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_CompanyId",
                table: "Drawers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_DefaultScreenId",
                table: "Drawers",
                column: "DefaultScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_DrawerGroupId",
                table: "Drawers",
                column: "DrawerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_StoreOfOperationId",
                table: "Drawers",
                column: "StoreOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Drawers_TerminalId",
                table: "Drawers",
                column: "TerminalId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawerGroups_CompanyId",
                table: "DrawerGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawerGroups_StoreOfOperationId",
                table: "DrawerGroups",
                column: "StoreOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultScreens_CompanyId",
                table: "DefaultScreens",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultScreens_StoreOfOperationId",
                table: "DefaultScreens",
                column: "StoreOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedDrawers_CompanyId",
                table: "AssignedDrawers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedDrawers_DefaultScreenId",
                table: "AssignedDrawers",
                column: "DefaultScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedDrawers_DrawerId",
                table: "AssignedDrawers",
                column: "DrawerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedDrawers_StoreOfOperationId",
                table: "AssignedDrawers",
                column: "StoreOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedDrawers_UserId",
                table: "AssignedDrawers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedDrawers_AspNetUsers_UserId",
                table: "AssignedDrawers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedDrawers_Companies_CompanyId",
                table: "AssignedDrawers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedDrawers_DefaultScreens_DefaultScreenId",
                table: "AssignedDrawers",
                column: "DefaultScreenId",
                principalTable: "DefaultScreens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedDrawers_Drawers_DrawerId",
                table: "AssignedDrawers",
                column: "DrawerId",
                principalTable: "Drawers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedDrawers_Stores_StoreOfOperationId",
                table: "AssignedDrawers",
                column: "StoreOfOperationId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultScreens_Companies_CompanyId",
                table: "DefaultScreens",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultScreens_Stores_StoreOfOperationId",
                table: "DefaultScreens",
                column: "StoreOfOperationId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DrawerGroups_Companies_CompanyId",
                table: "DrawerGroups",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DrawerGroups_Stores_StoreOfOperationId",
                table: "DrawerGroups",
                column: "StoreOfOperationId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drawers_Companies_CompanyId",
                table: "Drawers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drawers_DefaultScreens_DefaultScreenId",
                table: "Drawers",
                column: "DefaultScreenId",
                principalTable: "DefaultScreens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drawers_DrawerGroups_DrawerGroupId",
                table: "Drawers",
                column: "DrawerGroupId",
                principalTable: "DrawerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drawers_Stores_StoreOfOperationId",
                table: "Drawers",
                column: "StoreOfOperationId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drawers_Terminals_TerminalId",
                table: "Drawers",
                column: "TerminalId",
                principalTable: "Terminals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Terminals_Companies_CompanyId",
                table: "Terminals",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Terminals_Stores_StoreOfOperationId",
                table: "Terminals",
                column: "StoreOfOperationId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedDrawers_AspNetUsers_UserId",
                table: "AssignedDrawers");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignedDrawers_Companies_CompanyId",
                table: "AssignedDrawers");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignedDrawers_DefaultScreens_DefaultScreenId",
                table: "AssignedDrawers");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignedDrawers_Drawers_DrawerId",
                table: "AssignedDrawers");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignedDrawers_Stores_StoreOfOperationId",
                table: "AssignedDrawers");

            migrationBuilder.DropForeignKey(
                name: "FK_DefaultScreens_Companies_CompanyId",
                table: "DefaultScreens");

            migrationBuilder.DropForeignKey(
                name: "FK_DefaultScreens_Stores_StoreOfOperationId",
                table: "DefaultScreens");

            migrationBuilder.DropForeignKey(
                name: "FK_DrawerGroups_Companies_CompanyId",
                table: "DrawerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_DrawerGroups_Stores_StoreOfOperationId",
                table: "DrawerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Drawers_Companies_CompanyId",
                table: "Drawers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drawers_DefaultScreens_DefaultScreenId",
                table: "Drawers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drawers_DrawerGroups_DrawerGroupId",
                table: "Drawers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drawers_Stores_StoreOfOperationId",
                table: "Drawers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drawers_Terminals_TerminalId",
                table: "Drawers");

            migrationBuilder.DropForeignKey(
                name: "FK_Terminals_Companies_CompanyId",
                table: "Terminals");

            migrationBuilder.DropForeignKey(
                name: "FK_Terminals_Stores_StoreOfOperationId",
                table: "Terminals");

            migrationBuilder.DropIndex(
                name: "IX_Terminals_CompanyId",
                table: "Terminals");

            migrationBuilder.DropIndex(
                name: "IX_Terminals_StoreOfOperationId",
                table: "Terminals");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_CompanyId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_DefaultScreenId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_DrawerGroupId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_StoreOfOperationId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_Drawers_TerminalId",
                table: "Drawers");

            migrationBuilder.DropIndex(
                name: "IX_DrawerGroups_CompanyId",
                table: "DrawerGroups");

            migrationBuilder.DropIndex(
                name: "IX_DrawerGroups_StoreOfOperationId",
                table: "DrawerGroups");

            migrationBuilder.DropIndex(
                name: "IX_DefaultScreens_CompanyId",
                table: "DefaultScreens");

            migrationBuilder.DropIndex(
                name: "IX_DefaultScreens_StoreOfOperationId",
                table: "DefaultScreens");

            migrationBuilder.DropIndex(
                name: "IX_AssignedDrawers_CompanyId",
                table: "AssignedDrawers");

            migrationBuilder.DropIndex(
                name: "IX_AssignedDrawers_DefaultScreenId",
                table: "AssignedDrawers");

            migrationBuilder.DropIndex(
                name: "IX_AssignedDrawers_DrawerId",
                table: "AssignedDrawers");

            migrationBuilder.DropIndex(
                name: "IX_AssignedDrawers_StoreOfOperationId",
                table: "AssignedDrawers");

            migrationBuilder.DropIndex(
                name: "IX_AssignedDrawers_UserId",
                table: "AssignedDrawers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Terminals");

            migrationBuilder.DropColumn(
                name: "StoreOfOperationId",
                table: "Terminals");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "StoreOfOperationId",
                table: "Drawers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DrawerGroups");

            migrationBuilder.DropColumn(
                name: "StoreOfOperationId",
                table: "DrawerGroups");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DefaultScreens");

            migrationBuilder.DropColumn(
                name: "StoreOfOperationId",
                table: "DefaultScreens");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AssignedDrawers");

            migrationBuilder.DropColumn(
                name: "StoreOfOperationId",
                table: "AssignedDrawers");

            migrationBuilder.AlterColumn<Guid>(
                name: "TerminalId",
                table: "Drawers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DrawerGroupId",
                table: "Drawers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultScreenId",
                table: "Drawers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignedDrawers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DrawerId",
                table: "AssignedDrawers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultScreenId",
                table: "AssignedDrawers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
