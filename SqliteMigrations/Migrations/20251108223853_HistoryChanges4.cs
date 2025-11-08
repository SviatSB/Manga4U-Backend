using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class HistoryChanges4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChapter",
                table: "Histories");

            migrationBuilder.RenameColumn(
                name: "LastPage",
                table: "Histories",
                newName: "LastChapterNumber");

            migrationBuilder.AddColumn<string>(
                name: "TagExternalId",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Histories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastChapterId",
                table: "Histories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastChapterTitle",
                table: "Histories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MangaExternalId",
                table: "Histories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MangaName",
                table: "Histories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagExternalId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "LastChapterId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "LastChapterTitle",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "MangaExternalId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "MangaName",
                table: "Histories");

            migrationBuilder.RenameColumn(
                name: "LastChapterNumber",
                table: "Histories",
                newName: "LastPage");

            migrationBuilder.AddColumn<int>(
                name: "LastChapter",
                table: "Histories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
