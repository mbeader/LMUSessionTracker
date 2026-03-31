using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Vehicles130 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antti Rammo", "4_25_DKR_E8E7FBE8C" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "EE", "Bronze" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mikkel Gaarde Pedersen", "4_25_DKR_E8E7FBE8C" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "DK", "Silver" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Wyatt Brichacek", "4_25_DKR_E8E7FBE8C" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "US", "Silver" });

            migrationBuilder.InsertData(
                table: "VehicleModels",
                columns: new[] { "Id", "Engine", "Manufacturer", "Name" },
                values: new object[,]
                {
                    { "Duqueine_D09_P3", "Toyota V35A-FTS 3500 cc V6 twin-turbocharged", "Duqueine", "Duqueine D09 P3" },
                    { "Genesis_GMR001", "Genesis G8MR 3.2L V8 twin turbocharged", "Genesis", "Genesis GMR001" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Class", "Custom", "Livery", "Model", "Name", "Number", "Series", "Team" },
                values: new object[,]
                {
                    { "12_25_WTM_7FC250D8", "LMP3", false, "ELMS", "Duqueine_D09_P3", "WTM by Rinaldi Racing 2025 #12:ELMS", "12", "ELMS2025", "WTM by Rinaldi Racing" },
                    { "17_26_GENE14CD51B0", "Hypercar", false, "WEC", "Genesis_GMR001", "Genesis Magma Racing 2026 #17:WEC", "17", "WEC2026", "Genesis Magma Racing" },
                    { "397_25_D09P3", "LMP3", true, "Custom", "Duqueine_D09_P3", "Duqueine D09 P3 Custom Team 2025 #397", "397", "ELMS2025", "Custom Team" },
                    { "397_26_GMR001", "Hypercar", true, "Custom", "Genesis_GMR001", "Genesis Custom Team 2026 #397", "397", "WEC2026", "Custom Team" }
                });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Griffin Peebles", "12_25_WTM_7FC250D8", "AU", "Silver" },
                    { "Leonard Weiss", "12_25_WTM_7FC250D8", "DE", "Silver" },
                    { "Torsten Kratz", "12_25_WTM_7FC250D8", "DE", "Bronze" },
                    { "André Lotterer", "17_26_GENE14CD51B0", "DE", "Platinum" },
                    { "Mathys Jaubert", "17_26_GENE14CD51B0", "FR", "Silver" },
                    { "Pipo Derani", "17_26_GENE14CD51B0", "BR", "Platinum" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Griffin Peebles", "12_25_WTM_7FC250D8" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Leonard Weiss", "12_25_WTM_7FC250D8" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Torsten Kratz", "12_25_WTM_7FC250D8" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "André Lotterer", "17_26_GENE14CD51B0" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mathys Jaubert", "17_26_GENE14CD51B0" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Pipo Derani", "17_26_GENE14CD51B0" });

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_25_D09P3");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_26_GMR001");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "12_25_WTM_7FC250D8");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "17_26_GENE14CD51B0");

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Duqueine_D09_P3");

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Genesis_GMR001");

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Antti Rammo", "4_25_DKR_E8E7FBE8C" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mikkel Gaarde Pedersen", "4_25_DKR_E8E7FBE8C" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Wyatt Brichacek", "4_25_DKR_E8E7FBE8C" },
                columns: new[] { "Nationality", "Skill" },
                values: new object[] { "", "" });
        }
    }
}
