using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class PitResolved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Resolved",
                table: "Pits",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolved",
                table: "Pits");
        }
    }
}
