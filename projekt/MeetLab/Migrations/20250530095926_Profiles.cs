using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetLab.Migrations
{
    /// <inheritdoc />
    public partial class Profiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    NickName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.NickName);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 30, 11, 59, 26, 398, DateTimeKind.Local).AddTicks(7846));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 30, 0, 58, 56, 620, DateTimeKind.Local).AddTicks(5198));
        }
    }
}
