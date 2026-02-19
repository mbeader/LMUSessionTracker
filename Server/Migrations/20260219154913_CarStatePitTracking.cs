using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class CarStatePitTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GarageThisLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LastPitLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<double>(
                name: "LastPitTime",
                table: "CarStates",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<int>(
                name: "LastSwapLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<double>(
                name: "LastSwapTime",
                table: "CarStates",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<bool>(
                name: "PitThisLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SwapLocation",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<bool>(
                name: "SwapThisLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalPenalties",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPitstops",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GarageThisLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastPitLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastPitTime",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastSwapLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "LastSwapTime",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "PitThisLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "SwapLocation",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "SwapThisLap",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "TotalPenalties",
                table: "CarStates");

            migrationBuilder.DropColumn(
                name: "TotalPitstops",
                table: "CarStates");
        }
    }
}
