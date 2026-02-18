using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class LapStartTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "StartTime",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Laps");
        }
    }
}
