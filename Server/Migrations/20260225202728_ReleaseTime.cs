using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReleaseTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExitTime",
                table: "Pits",
                newName: "ReleaseTime");

            migrationBuilder.RenameColumn(
                name: "LastExitTime",
                table: "CarStates",
                newName: "LastReleaseTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReleaseTime",
                table: "Pits",
                newName: "ExitTime");

            migrationBuilder.RenameColumn(
                name: "LastReleaseTime",
                table: "CarStates",
                newName: "LastExitTime");
        }
    }
}
