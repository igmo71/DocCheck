using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocCheck.Migrations
{
    /// <inheritdoc />
    public partial class AddBitrixId : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BitrixId",
                table: "AspNetUsers");
        }
    }
}
