using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircuitManager.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CircuitElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MachineType = table.Column<int>(type: "INTEGER", nullable: false),
                    NextCircuitElementId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircuitElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircuitElements_CircuitElements_NextCircuitElementId",
                        column: x => x.NextCircuitElementId,
                        principalTable: "CircuitElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Shortcut = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CircuitElementComponents",
                columns: table => new
                {
                    CircuitElementsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComponentListId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircuitElementComponents", x => new { x.CircuitElementsId, x.ComponentListId });
                    table.ForeignKey(
                        name: "FK_CircuitElementComponents_CircuitElements_CircuitElementsId",
                        column: x => x.CircuitElementsId,
                        principalTable: "CircuitElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CircuitElementComponents_Components_ComponentListId",
                        column: x => x.ComponentListId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CircuitElementComponents_ComponentListId",
                table: "CircuitElementComponents",
                column: "ComponentListId");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitElements_NextCircuitElementId",
                table: "CircuitElements",
                column: "NextCircuitElementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CircuitElementComponents");

            migrationBuilder.DropTable(
                name: "CircuitElements");

            migrationBuilder.DropTable(
                name: "Components");
        }
    }
}
