using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class CarStateStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPitstops",
                table: "CarStates",
                newName: "TotalStops");

            migrationBuilder.AddColumn<double>(
                name: "LastExitTime",
                table: "CarStates",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<int>(
                name: "LastStopLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<double>(
                name: "LastStopTime",
                table: "CarStates",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<bool>(
                name: "StartedLapInPit",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StopThisLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalPits",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastExitTime",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastStopLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastStopTime",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "StartedLapInPit",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "StopThisLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "TotalPits",
                table: "CarStates");

            migrationBuilder.RenameColumn(
                name: "TotalStops",
                table: "CarStates",
                newName: "TotalPitstops");
        }
    }
}
