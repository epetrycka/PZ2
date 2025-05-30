using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetLab.Migrations
{
    /// <inheritdoc />
    public partial class InitMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SenderNick = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverNick = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    SentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 30, 0, 23, 6, 983, DateTimeKind.Local).AddTicks(8260));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 29, 23, 58, 48, 240, DateTimeKind.Local).AddTicks(4494));
        }
    }
}
