using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetLab.Migrations
{
    /// <inheritdoc />
    public partial class Nwm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 30, 11, 15, 52, 846, DateTimeKind.Local).AddTicks(1616));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 30, 0, 58, 56, 620, DateTimeKind.Local).AddTicks(5198));
        }
    }
}
