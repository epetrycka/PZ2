using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetLab.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sender = table.Column<string>(type: "TEXT", nullable: false),
                    Receiver = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    NickName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.NickName);
                });

            migrationBuilder.CreateTable(
                name: "UserFriends",
                columns: table => new
                {
                    FriendsNickName = table.Column<string>(type: "TEXT", nullable: false),
                    UserNickName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriends", x => new { x.FriendsNickName, x.UserNickName });
                    table.ForeignKey(
                        name: "FK_UserFriends_Users_FriendsNickName",
                        column: x => x.FriendsNickName,
                        principalTable: "Users",
                        principalColumn: "NickName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFriends_Users_UserNickName",
                        column: x => x.UserNickName,
                        principalTable: "Users",
                        principalColumn: "NickName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "NickName", "FirstName", "Password", "RegistrationDate", "Token" },
                values: new object[] { "admin", "Admin", "E64B78FC3BC91BCBC7DC232BA8EC59E0", new DateTime(2025, 5, 29, 23, 58, 48, 240, DateTimeKind.Local).AddTicks(4494), "Tly5g7BSn6dRHI0DdwJ63a3irvxsFN1qIwdzKypRAc0=" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFriends_UserNickName",
                table: "UserFriends",
                column: "UserNickName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.DropTable(
                name: "UserFriends");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
