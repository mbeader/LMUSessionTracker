using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class LapResolved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LFTire",
                table: "Laps",
                newName: "LFUsage");

            migrationBuilder.RenameColumn(
                name: "LRTire",
                table: "Laps",
                newName: "LRUsage");

            migrationBuilder.RenameColumn(
                name: "RFTire",
                table: "Laps",
                newName: "RFUsage");

            migrationBuilder.RenameColumn(
                name: "RRTire",
                table: "Laps",
                newName: "RRUsage");

            migrationBuilder.AlterColumn<double>(
                name: "VirtualEnergy",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "Fuel",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "LFUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "LRUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "RFUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "RRUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: -1.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<bool>(
                name: "Resolved",
                table: "Laps",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolved",
                table: "Laps");

            migrationBuilder.AlterColumn<double>(
                name: "VirtualEnergy",
                table: "Laps",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldDefaultValue: -1.0);

            migrationBuilder.AlterColumn<double>(
                name: "Fuel",
                table: "Laps",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldDefaultValue: -1.0);

            migrationBuilder.AlterColumn<double>(
                name: "LFUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "LRUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "RFUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<double>(
                name: "RRUsage",
                table: "Laps",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.RenameColumn(
                name: "LFUsage",
                table: "Laps",
                newName: "LFTire");

            migrationBuilder.RenameColumn(
                name: "LRUsage",
                table: "Laps",
                newName: "LRTire");

            migrationBuilder.RenameColumn(
                name: "RFUsage",
                table: "Laps",
                newName: "RFTire");

            migrationBuilder.RenameColumn(
                name: "RRUsage",
                table: "Laps",
                newName: "RRTire");
        }
    }
}
