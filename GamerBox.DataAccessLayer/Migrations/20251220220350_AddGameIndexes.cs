using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamerBox.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddGameIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Games_Genre",
                table: "Games",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Title",
                table: "Games",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_Genre",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Title",
                table: "Games");
        }
    }
}
