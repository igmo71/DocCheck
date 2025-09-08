using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocCheck.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOriginalDocumentReceived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOriginalDocumentReceived",
                table: "SaleDocs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOriginalDocumentReceived",
                table: "SaleDocs");
        }
    }
}
