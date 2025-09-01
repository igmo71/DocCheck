using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocCheck.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BitrixId",
                table: "AspNetUsers",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "AspNetUsers",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SaleDocLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    SaleDocId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Log = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDocLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDocLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleDocs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Redispatch = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDocs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaperworkErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaleDocId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaperworkErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaperworkErrors_SaleDocs_SaleDocId",
                        column: x => x.SaleDocId,
                        principalTable: "SaleDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuantityErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaleDocId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuantityErrors_SaleDocs_SaleDocId",
                        column: x => x.SaleDocId,
                        principalTable: "SaleDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaperworkErrors_SaleDocId",
                table: "PaperworkErrors",
                column: "SaleDocId");

            migrationBuilder.CreateIndex(
                name: "IX_QuantityErrors_SaleDocId",
                table: "QuantityErrors",
                column: "SaleDocId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDocLogs_UserId",
                table: "SaleDocLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDocs_UserId",
                table: "SaleDocs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaperworkErrors");

            migrationBuilder.DropTable(
                name: "QuantityErrors");

            migrationBuilder.DropTable(
                name: "SaleDocLogs");

            migrationBuilder.DropTable(
                name: "SaleDocs");

            migrationBuilder.DropColumn(
                name: "BitrixId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "AspNetUsers");
        }
    }
}
