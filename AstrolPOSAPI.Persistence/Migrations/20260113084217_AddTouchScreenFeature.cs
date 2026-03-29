using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AstrolPOSAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTouchScreenFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TouchScreens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScreenName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GridRows = table.Column<int>(type: "int", nullable: false),
                    GridColumns = table.Column<int>(type: "int", nullable: false),
                    DefaultFontSize = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreOfOperationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouchScreens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouchScreens_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TouchScreens_Stores_StoreOfOperationId",
                        column: x => x.StoreOfOperationId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TouchScreenButtons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TouchScreenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ButtonType = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Shape = table.Column<int>(type: "int", nullable: false),
                    BackgroundColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    TextColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    FontSize = table.Column<int>(type: "int", nullable: true),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    RowSpan = table.Column<int>(type: "int", nullable: false),
                    ColumnSpan = table.Column<int>(type: "int", nullable: false),
                    ShowImage = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsDefaultImage = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreOfOperationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouchScreenButtons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouchScreenButtons_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TouchScreenButtons_Stores_StoreOfOperationId",
                        column: x => x.StoreOfOperationId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TouchScreenButtons_TouchScreens_TouchScreenId",
                        column: x => x.TouchScreenId,
                        principalTable: "TouchScreens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TouchScreenButtons_CompanyId",
                table: "TouchScreenButtons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TouchScreenButtons_Row_Column",
                table: "TouchScreenButtons",
                columns: new[] { "Row", "Column" });

            migrationBuilder.CreateIndex(
                name: "IX_TouchScreenButtons_StoreOfOperationId",
                table: "TouchScreenButtons",
                column: "StoreOfOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_TouchScreenButtons_TouchScreenId",
                table: "TouchScreenButtons",
                column: "TouchScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_TouchScreens_CompanyId",
                table: "TouchScreens",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TouchScreens_StoreOfOperationId",
                table: "TouchScreens",
                column: "StoreOfOperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TouchScreenButtons");

            migrationBuilder.DropTable(
                name: "TouchScreens");
        }
    }
}
