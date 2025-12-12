using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class comentsetnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_RepliedCommentId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_RepliedCommentId",
                table: "Comments",
                column: "RepliedCommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_RepliedCommentId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_RepliedCommentId",
                table: "Comments",
                column: "RepliedCommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
