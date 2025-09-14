using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CircuitManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MachineTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CircuitElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MachineTypeId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_CircuitElements_MachineTypes_MachineTypeId",
                        column: x => x.MachineTypeId,
                        principalTable: "MachineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.InsertData(
                table: "Components",
                columns: new[] { "Id", "Direction", "Label", "Name" },
                values: new object[,]
                {
                    { 1, 1, "N", "Napęd" },
                    { 2, 0, "P", "Przycisk" },
                    { 3, 1, "S", "Stycznik" },
                    { 4, 0, "C", "Czujnik" },
                    { 5, 1, "R", "Przenośnik" },
                    { 6, 0, "K", "Korek" }
                });

            migrationBuilder.InsertData(
                table: "MachineTypes",
                columns: new[] { "Id", "Label", "Name" },
                values: new object[,]
                {
                    { 1, "TP", "Transporter Palet" },
                    { 2, "TO", "Obrotnica" },
                    { 3, "MP", "Magazyn Palet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CircuitElementComponents_ComponentListId",
                table: "CircuitElementComponents",
                column: "ComponentListId");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitElements_MachineTypeId",
                table: "CircuitElements",
                column: "MachineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitElements_Name",
                table: "CircuitElements",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CircuitElements_NextCircuitElementId",
                table: "CircuitElements",
                column: "NextCircuitElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_Label",
                table: "Components",
                column: "Label",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_Name",
                table: "Components",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MachineTypes_Label",
                table: "MachineTypes",
                column: "Label",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MachineTypes_Name",
                table: "MachineTypes",
                column: "Name",
                unique: true);
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

            migrationBuilder.DropTable(
                name: "MachineTypes");
        }
    }
}
