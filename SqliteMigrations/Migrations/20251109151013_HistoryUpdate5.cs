using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class HistoryUpdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MangaName",
                table: "Histories");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "Tags");

            migrationBuilder.AddColumn<string>(
                name: "MangaName",
                table: "Histories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
