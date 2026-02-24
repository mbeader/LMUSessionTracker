using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class CarStatePitMissingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ExitTime",
                table: "Pits",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0);

            migrationBuilder.AddColumn<bool>(
                name: "PenaltyThisLap",
                table: "CarStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExitTime",
                table: "Pits");

            migrationBuilder.DropColumn(
                name: "PenaltyThisLap",
                table: "CarStates");
        }
    }
}
