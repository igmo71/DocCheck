using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocCheck.Migrations
{
    /// <inheritdoc />
    public partial class CreateDocumentCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentCheck",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InvoiceRefKey = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsInvoiceCorrection = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCheck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentCheck_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCheckLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Log = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCheckLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Error",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Error", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Error_DocumentCheck_DocumentCheckId",
                        column: x => x.DocumentCheckId,
                        principalTable: "DocumentCheck",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCheck_UserId",
                table: "DocumentCheck",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Error_DocumentCheckId",
                table: "Error",
                column: "DocumentCheckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentCheckLog");

            migrationBuilder.DropTable(
                name: "Error");

            migrationBuilder.DropTable(
                name: "DocumentCheck");
        }
    }
}
