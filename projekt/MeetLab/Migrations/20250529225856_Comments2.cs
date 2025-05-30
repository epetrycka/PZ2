using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetLab.Migrations
{
    /// <inheritdoc />
    public partial class Comments2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Posts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 30, 0, 58, 56, 620, DateTimeKind.Local).AddTicks(5198));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "NickName",
                keyValue: "admin",
                column: "RegistrationDate",
                value: new DateTime(2025, 5, 30, 0, 53, 38, 637, DateTimeKind.Local).AddTicks(8767));
        }
    }
}
