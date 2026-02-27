using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class CarStateMoreTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartedLapInPit",
                table: "CarStates");

            migrationBuilder.AddColumn<double>(
                name: "ExitTime",
                table: "Pits",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<double>(
                name: "GarageInTime",
                table: "Pits",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<double>(
                name: "GarageOutTime",
                table: "Pits",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<int>(
                name: "StopLocation",
                table: "Pits",
                type: "INTEGER",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<double>(
                name: "LastExitTime",
                table: "CarStates",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<double>(
                name: "LastGarageInTime",
                table: "CarStates",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<int>(
                name: "LastGarageLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<double>(
                name: "LastGarageOutTime",
                table: "CarStates",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<string>(
                name: "LastLapEndPitState",
                table: "CarStates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StopLocation",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<string>(
                name: "ThisLapStartPitState",
                table: "CarStates",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExitTime",
                table: "Pits");

            migrationBuilder.DropColumn(
                name: "GarageInTime",
                table: "Pits");

            migrationBuilder.DropColumn(
                name: "GarageOutTime",
                table: "Pits");

            migrationBuilder.DropColumn(
                name: "StopLocation",
                table: "Pits");

            migrationBuilder.DropColumn(
                name: "LastExitTime",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastGarageInTime",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastGarageLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastGarageOutTime",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastLapEndPitState",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "StopLocation",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "ThisLapStartPitState",
                table: "CarStates");

            migrationBuilder.AddColumn<bool>(
                name: "StartedLapInPit",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
