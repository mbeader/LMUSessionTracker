using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class PeugeotWing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VehicleModels",
                columns: new[] { "Id", "Engine", "Manufacturer", "Name" },
                values: new object[] { "Peugeot_9x8_Wing", "Peugeot", "Peugeot X6H 2.6 litre V6 90 degree twin-turbo", "Peugeot 9x8 Wing" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_24_9X8W",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_25_9X8W",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_24_PEUG98B27A84",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_24_PEUGE6B75DF3",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_25_PEUG354B9ECA",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_25_PEUGD0A1B532",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_24_PEUG511E4063",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_24_PEUGA53E1BA4",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_25_PEUG5155917B",
                column: "Model",
                value: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_25_PEUGAEF0BB48",
                column: "Model",
                value: "Peugeot_9x8_Wing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: "Peugeot_9x8_Wing");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_24_9X8W",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "397_25_9X8W",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_24_PEUG98B27A84",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_24_PEUGE6B75DF3",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_25_PEUG354B9ECA",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "93_25_PEUGD0A1B532",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_24_PEUG511E4063",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_24_PEUGA53E1BA4",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_25_PEUG5155917B",
                column: "Model",
                value: "Peugeot_9x8");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: "94_25_PEUGAEF0BB48",
                column: "Model",
                value: "Peugeot_9x8");
        }
    }
}
