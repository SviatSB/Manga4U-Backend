using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class comments1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Mangas_MangaId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MangaId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "MangaId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "ChapterExternalId",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterExternalId",
                table: "Comments");

            migrationBuilder.AddColumn<long>(
                name: "MangaId",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MangaId",
                table: "Comments",
                column: "MangaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Mangas_MangaId",
                table: "Comments",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
