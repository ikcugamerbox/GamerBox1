using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamerBox.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameUserList",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    UserListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUserList", x => new { x.GamesId, x.UserListId });
                    table.ForeignKey(
                        name: "FK_GameUserList_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameUserList_UserLists_UserListId",
                        column: x => x.UserListId,
                        principalTable: "UserLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameUserList_UserListId",
                table: "GameUserList",
                column: "UserListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLists_UserId",
                table: "UserLists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameUserList");

            migrationBuilder.DropTable(
                name: "UserLists");
        }
    }
}
