using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMUSessionTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChatTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Temp",
                table: "Chats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Nanoseconds",
                table: "Chats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql("UPDATE Chats SET Temp = unixepoch(Timestamp), Nanoseconds = CAST(substring(Timestamp, 21) as INTEGER)*100;");

            migrationBuilder.DropIndex(
                name: "IX_Chats_Timestamp_Message",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "Temp",
                table: "Chats",
                newName: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SessionId_Timestamp_Nanoseconds_Message",
                table: "Chats",
                columns: new[] { "SessionId", "Timestamp", "Nanoseconds", "Message" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Temp",
                table: "Chats",
                type: "TEXT",
                nullable: false,
                defaultValue: "1970-01-01T00:00:00.0000000");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Chats");

            migrationBuilder.Sql("UPDATE Chats SET Temp = strftime('%FT%T', Timestamp, 'unixepoch') || '.' || substring(CAST(Nanoseconds as TEXT), 0, 8);");

            migrationBuilder.DropIndex(
                name: "IX_Chats_SessionId_Timestamp_Nanoseconds_Message",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "Nanoseconds",
                table: "Chats");
            
            migrationBuilder.RenameColumn(
                name: "Temp",
                table: "Chats",
                newName: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Timestamp_Message",
                table: "Chats",
                columns: new[] { "Timestamp", "Message" },
                unique: true);
        }
    }
}
