using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircuitManager.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Components_Name",
                table: "Components",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CircuitElements_Name",
                table: "CircuitElements",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Components_Name",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_CircuitElements_Name",
                table: "CircuitElements");
        }
    }
}
