using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class SessionTransition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionTransitions",
                columns: table => new
                {
                    SessionTransitionId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FromSessionId = table.Column<string>(type: "TEXT", nullable: false),
                    ToSessionId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTransitions", x => x.SessionTransitionId);
                    table.UniqueConstraint("AK_SessionTransitions_FromSessionId_ToSessionId", x => new { x.FromSessionId, x.ToSessionId });
                    table.ForeignKey(
                        name: "FK_SessionTransitions_Sessions_FromSessionId",
                        column: x => x.FromSessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionTransitions_Sessions_ToSessionId",
                        column: x => x.ToSessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionTransitions_ToSessionId",
                table: "SessionTransitions",
                column: "ToSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionTransitions");
        }
    }
}
