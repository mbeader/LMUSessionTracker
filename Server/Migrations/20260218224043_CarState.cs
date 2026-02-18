using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class CarState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarStates",
                columns: table => new
                {
                    CarStateId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarId = table.Column<long>(type: "INTEGER", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    CountLapFlag = table.Column<string>(type: "TEXT", nullable: true),
                    DriverName = table.Column<string>(type: "TEXT", nullable: true),
                    FinishStatus = table.Column<string>(type: "TEXT", nullable: true),
                    InGarageStall = table.Column<bool>(type: "INTEGER", nullable: false),
                    LapStartET = table.Column<double>(type: "REAL", nullable: false),
                    LapsCompleted = table.Column<int>(type: "INTEGER", nullable: false),
                    Penalties = table.Column<int>(type: "INTEGER", nullable: false),
                    PitState = table.Column<string>(type: "TEXT", nullable: true),
                    Pitstops = table.Column<int>(type: "INTEGER", nullable: false),
                    Pitting = table.Column<bool>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    ServerScored = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarStates", x => x.CarStateId);
                    table.ForeignKey(
                        name: "FK_CarStates_Cars_SessionId_CarId",
                        columns: x => new { x.SessionId, x.CarId },
                        principalTable: "Cars",
                        principalColumns: new[] { "SessionId", "CarId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarStates_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarStates_SessionId_CarId",
                table: "CarStates",
                columns: new[] { "SessionId", "CarId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarStates");
        }
    }
}
