using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SimpleMVC.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dane",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tekst = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dane", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loginy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoginName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loginy", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Dane",
                columns: new[] { "Id", "Tekst" },
                values: new object[,]
                {
                    { 1, "To jest pierwszy komentarz." },
                    { 2, "Drugi przykładowy wpis." },
                    { 3, "Trzeci tekst testowy." }
                });

            migrationBuilder.InsertData(
                table: "Loginy",
                columns: new[] { "Id", "LoginName", "Password" },
                values: new object[,]
                {
                    { 1, "admin", "E64B78FC3BC91BCBC7DC232BA8EC59E0" },
                    { 2, "user", "6EDF26F6E0BADFF12FCA32B16DB38BF2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dane");

            migrationBuilder.DropTable(
                name: "Loginy");
        }
    }
}
