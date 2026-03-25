using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class LapTireCompounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LFCompound",
                table: "Laps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LRCompound",
                table: "Laps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RFCompound",
                table: "Laps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RRCompound",
                table: "Laps",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LFCompound",
                table: "Laps");

            migrationBuilder.DropColumn(
                name: "LRCompound",
                table: "Laps");

            migrationBuilder.DropColumn(
                name: "RFCompound",
                table: "Laps");

            migrationBuilder.DropColumn(
                name: "RRCompound",
                table: "Laps");
        }
    }
}
