using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancePlus.Migrations
{
    /// <inheritdoc />
    public partial class acildeneme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VarlikToplami",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VarlikToplami",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
