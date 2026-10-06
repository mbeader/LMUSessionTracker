using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Vehicles143 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "21_26_AFCO31910023",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corse 2026 #21:LM", "Vista AF Corse" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_AFCO54087704",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corse 2026 #54:LM", "Vista AF Corse" });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Class", "Custom", "Livery", "Model", "Name", "Number", "Series", "Team" },
                values: new object[,]
                {
                    { "46_26_ADES20619231", "LMP3", false, "LMU1", "ADESS_AD25_LMP3", "ADESS Racing Team 2026 #46:LMU1", "46", "ELMS2026", "ADESS Racing Team 2026" },
                    { "46_26_ADES51981969", "LMP3", false, "ELMS", "ADESS_AD25_LMP3", "ADESS Racing Team 2026 #46:ELMS", "46", "ELMS2026", "ADESS Racing Team 2026" },
                    { "46_26_ADES81662442", "LMP3", false, "LMU2", "ADESS_AD25_LMP3", "ADESS Racing Team 2026 #46:LMU2", "46", "ELMS2026", "ADESS Racing Team 2026" }
                });

            migrationBuilder.InsertData(
                table: "VehicleDrivers",
                columns: new[] { "Name", "Veh", "Nationality", "Skill" },
                values: new object[,]
                {
                    { "Alex Sawczuk", "46_26_ADES20619231", "", "" },
                    { "Mirza Rustemović", "46_26_ADES20619231", "", "" },
                    { "Will Bennett", "46_26_ADES20619231", "", "" },
                    { "Alex Coutie", "46_26_ADES51981969", "", "" },
                    { "Paulo Matias", "46_26_ADES51981969", "", "" },
                    { "Stephen Haley", "46_26_ADES51981969", "", "" },
                    { "Dennis Jordan", "46_26_ADES81662442", "", "" },
                    { "Marek Lesniak", "46_26_ADES81662442", "", "" },
                    { "Michael Borda", "46_26_ADES81662442", "", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Sawczuk", "46_26_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Mirza Rustemović", "46_26_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Will Bennett", "46_26_ADES20619231" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Alex Coutie", "46_26_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Paulo Matias", "46_26_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Stephen Haley", "46_26_ADES51981969" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Dennis Jordan", "46_26_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Marek Lesniak", "46_26_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "VehicleDrivers",
                keyColumns: new[] { "Name", "Veh" },
                keyValues: new object[] { "Michael Borda", "46_26_ADES81662442" });

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_26_ADES20619231");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_26_ADES51981969");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "46_26_ADES81662442");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "21_26_AFCO31910023",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corsa 2026 #21:LM", "Vista AF Corsa" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "54_26_AFCO54087704",
                columns: new[] { "Name", "Team" },
                values: new object[] { "Vista AF Corsa 2026 #54:LM", "Vista AF Corsa" });
        }
    }
}
