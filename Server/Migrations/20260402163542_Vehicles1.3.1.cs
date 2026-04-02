using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Vehicles131 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Class", "Custom", "Livery", "Model", "Name", "Number", "Series", "Team" },
                values: new object[] { "19_26_GENE467AE018", "Hypercar", false, "WEC", "Genesis_GMR001", "Genesis Magma Racing 2026 #19:WEC", "19", "WEC2025", "Genesis Magma Racing" });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Daniel Juncadella", "19_26_GENE467AE018", "ES", "Platinum" },
                    { "Mathieu Jaminet", "19_26_GENE467AE018", "FR", "Platinum" },
                    { "Paul-Loup Chatin", "19_26_GENE467AE018", "FR", "Gold" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Daniel Juncadella", "19_26_GENE467AE018" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mathieu Jaminet", "19_26_GENE467AE018" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paul-Loup Chatin", "19_26_GENE467AE018" });

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "19_26_GENE467AE018");
        }
    }
}
