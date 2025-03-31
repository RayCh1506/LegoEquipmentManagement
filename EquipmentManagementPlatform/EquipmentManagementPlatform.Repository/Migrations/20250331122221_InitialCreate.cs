using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EquipmentManagementPlatform.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOperational = table.Column<bool>(type: "bit", nullable: false),
                    FaultMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentOrder = table.Column<int>(type: "int", nullable: true),
                    AssignedOrders = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverallEquipmentEffectiveness = table.Column<double>(type: "float", nullable: false),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentHistoricState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeOfChange = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentHistoricState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentEntityEquipmentHistoricStateEntity",
                columns: table => new
                {
                    EquipmentEntityId = table.Column<int>(type: "int", nullable: false),
                    HistoricStatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentEntityEquipmentHistoricStateEntity", x => new { x.EquipmentEntityId, x.HistoricStatesId });
                    table.ForeignKey(
                        name: "FK_EquipmentEntityEquipmentHistoricStateEntity_EquipmentHistoricState_HistoricStatesId",
                        column: x => x.HistoricStatesId,
                        principalTable: "EquipmentHistoricState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentEntityEquipmentHistoricStateEntity_Equipment_EquipmentEntityId",
                        column: x => x.EquipmentEntityId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "Id", "AssignedOrders", "CurrentOrder", "FaultMessage", "IsOperational", "Location", "Name", "Operator", "OverallEquipmentEffectiveness", "State" },
                values: new object[,]
                {
                    { 1, "[5443]", 5443, "", true, "A1.01", "EQ758490", "EMP123 - FirstName1 LastName1", 98.200000000000003, "GREEN" },
                    { 2, "[]", null, "Machine is blocked", false, "A2.11", "EQ125896", "EMP666 - FirstName2 LastName2 ", 12.0, "RED" },
                    { 3, "[]", null, "", true, "B3.11", "EQ147852", "EMP006 - FirstName3 LastName3 ", 76.5, "YELLOW" },
                    { 4, "[1111]", 1111, "", true, "B3.07", "EQ125987", "EMP223 - FirstName4 LastName4 ", 96.700000000000003, "GREEN" },
                    { 5, "[]", null, "", true, "B3.01", "EQ136574", "EMP874 - FirstName5 LastName5 ", 77.900000000000006, "RED" },
                    { 6, "[]", null, "", true, "C2.02", "EQ174444", "", 84.400000000000006, "YELLOW" },
                    { 7, "[2222]", 2222, "", true, "C3.03", "EQ195874", "EMP999 - FirstName6 LastName6 ", 99.5, "GREEN" },
                    { 8, "[]", null, "No power", false, "C1.05", "EQ132132", "", 41.200000000000003, "RED" },
                    { 9, "[]", null, "", true, "D1.09", "EQ456484", "", 86.900000000000006, "YELLOW" },
                    { 10, "[3333]", 3333, "", true, "D1.05", "EQ741874", "EMP365 - FirstName7 LastName7 ", 81.400000000000006, "GREEN" },
                    { 11, "[]", null, "", true, "D1.03", "EQ896214", "EMP547 - FirstName8 LastName8 ", 86.200000000000003, "RED" },
                    { 12, "[]", null, "", true, "E2.01", "EQ745635", "EMP874 - - FirstName9 LastName9 ", 77.0, "YELLOW" },
                    { 13, "[4444]", 4444, "", true, "E2.02", "EQ555555", "EMP852 - FirstName10 LastName10 ", 79.400000000000006, "GREEN" },
                    { 14, "[]", null, "A very long error message about the equipment fault", false, "E2.03", "EQ785412", "EMP854 - FirstName11 LastName11  ", 6.4000000000000004, "RED" },
                    { 15, "[]", null, "", true, "F6.11", "EQ951375", "EMP965 - FirstName12 LastName12 ", 66.700000000000003, "YELLOW" },
                    { 16, "[5555]", 5555, "", true, "F7.17", "EQ749625", "EMP148 - FirstName13 LastName13 ", 83.599999999999994, "GREEN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentEntityEquipmentHistoricStateEntity_HistoricStatesId",
                table: "EquipmentEntityEquipmentHistoricStateEntity",
                column: "HistoricStatesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentEntityEquipmentHistoricStateEntity");

            migrationBuilder.DropTable(
                name: "EquipmentHistoricState");

            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
