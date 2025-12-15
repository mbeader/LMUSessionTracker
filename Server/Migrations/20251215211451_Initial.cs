using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsClosed = table.Column<bool>(type: "INTEGER", nullable: false),
                    EndEventTime = table.Column<double>(type: "REAL", nullable: false),
                    GameMode = table.Column<string>(type: "TEXT", nullable: true),
                    LapDistance = table.Column<double>(type: "REAL", nullable: false),
                    MaxPlayers = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxTime = table.Column<double>(type: "REAL", nullable: false),
                    MaximumLaps = table.Column<uint>(type: "INTEGER", nullable: false),
                    PasswordProtected = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: true),
                    ServerPort = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionType = table.Column<string>(type: "TEXT", nullable: true),
                    StartEventTime = table.Column<double>(type: "REAL", nullable: false),
                    TrackName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_Chats_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    EntryId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    SlotId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Number = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Vehicle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_Entries_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Laps",
                columns: table => new
                {
                    LapId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    SlotId = table.Column<int>(type: "INTEGER", nullable: false),
                    Veh = table.Column<string>(type: "TEXT", nullable: false),
                    LapNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CarClass = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle = table.Column<string>(type: "TEXT", nullable: true),
                    Team = table.Column<string>(type: "TEXT", nullable: true),
                    Driver = table.Column<string>(type: "TEXT", nullable: true),
                    FinishStatus = table.Column<string>(type: "TEXT", nullable: true),
                    TotalTime = table.Column<double>(type: "REAL", nullable: false),
                    Sector1 = table.Column<double>(type: "REAL", nullable: false),
                    Sector2 = table.Column<double>(type: "REAL", nullable: false),
                    Sector3 = table.Column<double>(type: "REAL", nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    Pit = table.Column<bool>(type: "INTEGER", nullable: false),
                    Fuel = table.Column<double>(type: "REAL", nullable: false),
                    VirtualEnergy = table.Column<double>(type: "REAL", nullable: false),
                    LFTire = table.Column<double>(type: "REAL", nullable: false),
                    RFTire = table.Column<double>(type: "REAL", nullable: false),
                    LRTire = table.Column<double>(type: "REAL", nullable: false),
                    RRTire = table.Column<double>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laps", x => x.LapId);
                    table.ForeignKey(
                        name: "FK_Laps_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionStates",
                columns: table => new
                {
                    SessionStateId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AmbientTemp = table.Column<double>(type: "REAL", nullable: false),
                    AveragePathWetness = table.Column<double>(type: "REAL", nullable: false),
                    CurrentEventTime = table.Column<double>(type: "REAL", nullable: false),
                    DarkCloud = table.Column<double>(type: "REAL", nullable: false),
                    GamePhase = table.Column<int>(type: "INTEGER", nullable: true),
                    InRealtime = table.Column<bool>(type: "INTEGER", nullable: true),
                    MaxPathWetness = table.Column<double>(type: "REAL", nullable: false),
                    MinPathWetness = table.Column<double>(type: "REAL", nullable: false),
                    NumRedLights = table.Column<int>(type: "INTEGER", nullable: true),
                    NumberOfPlayers = table.Column<int>(type: "INTEGER", nullable: true),
                    NumberOfVehicles = table.Column<int>(type: "INTEGER", nullable: true),
                    RaceCompletion = table.Column<double>(type: "REAL", nullable: true),
                    Raining = table.Column<double>(type: "REAL", nullable: true),
                    Sector1Flag = table.Column<string>(type: "TEXT", nullable: true),
                    Sector2Flag = table.Column<string>(type: "TEXT", nullable: true),
                    Sector3Flag = table.Column<string>(type: "TEXT", nullable: true),
                    StartLightFrame = table.Column<int>(type: "INTEGER", nullable: true),
                    TimeRemainingInGamePhase = table.Column<double>(type: "REAL", nullable: true),
                    TrackTemp = table.Column<double>(type: "REAL", nullable: true),
                    WindVelocity = table.Column<double>(type: "REAL", nullable: true),
                    WindX = table.Column<double>(type: "REAL", nullable: true),
                    WindY = table.Column<double>(type: "REAL", nullable: true),
                    WindZ = table.Column<double>(type: "REAL", nullable: true),
                    YellowFlagState = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionStates", x => x.SessionStateId);
                    table.ForeignKey(
                        name: "FK_SessionStates_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntryId = table.Column<long>(type: "INTEGER", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Badge = table.Column<string>(type: "TEXT", nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", nullable: true),
                    IsDriver = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEngineer = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SessionId",
                table: "Chats",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Timestamp_Message",
                table: "Chats",
                columns: new[] { "Timestamp", "Message" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SessionId",
                table: "Entries",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Laps_SessionId",
                table: "Laps",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_EntryId",
                table: "Members",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_SessionId",
                table: "Members",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionStates_SessionId",
                table: "SessionStates",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Laps");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "SessionStates");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
