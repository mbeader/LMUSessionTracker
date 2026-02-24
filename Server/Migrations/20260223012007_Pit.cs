using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Pit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Garage",
                table: "Laps",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Penalty",
                table: "Laps",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Pits",
                columns: table => new
                {
                    PitId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    CarId = table.Column<long>(type: "INTEGER", nullable: false),
                    Lap = table.Column<int>(type: "INTEGER", nullable: false),
                    PitTime = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    StopTime = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    SwapTime = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    StopAfterLine = table.Column<bool>(type: "INTEGER", nullable: false),
                    Swap = table.Column<bool>(type: "INTEGER", nullable: false),
                    SwapLocation = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: -1),
                    Penalty = table.Column<bool>(type: "INTEGER", nullable: false),
                    Fuel = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    VirtualEnergy = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    LFChanged = table.Column<bool>(type: "INTEGER", nullable: false),
                    LFCompound = table.Column<string>(type: "TEXT", nullable: true),
                    LFNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    LFUsage = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    RFChanged = table.Column<bool>(type: "INTEGER", nullable: false),
                    RFCompound = table.Column<string>(type: "TEXT", nullable: true),
                    RFNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    RFUsage = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    LRChanged = table.Column<bool>(type: "INTEGER", nullable: false),
                    LRCompound = table.Column<string>(type: "TEXT", nullable: true),
                    LRNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    LRUsage = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    RRChanged = table.Column<bool>(type: "INTEGER", nullable: false),
                    RRCompound = table.Column<string>(type: "TEXT", nullable: true),
                    RRNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    RRUsage = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    PreviousStintDuration = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0),
                    Time = table.Column<double>(type: "REAL", nullable: false, defaultValue: -1.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pits", x => x.PitId);
                    table.ForeignKey(
                        name: "FK_Pits_Cars_SessionId_CarId",
                        columns: x => new { x.SessionId, x.CarId },
                        principalTable: "Cars",
                        principalColumns: new[] { "SessionId", "CarId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pits_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pits_SessionId_CarId",
                table: "Pits",
                columns: new[] { "SessionId", "CarId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pits");

            migrationBuilder.DropColumn(
                name: "Garage",
                table: "Laps");

            migrationBuilder.DropColumn(
                name: "Penalty",
                table: "Laps");
        }
    }
}
