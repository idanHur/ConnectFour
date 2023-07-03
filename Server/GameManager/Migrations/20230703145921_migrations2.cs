using Microsoft.EntityFrameworkCore.Migrations;

namespace GameManager.Migrations
{
    public partial class migrations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_playerId1",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_playerId1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "playerId1",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "playerId1",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_playerId1",
                table: "Games",
                column: "playerId1",
                unique: true,
                filter: "[playerId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_playerId1",
                table: "Games",
                column: "playerId1",
                principalTable: "Players",
                principalColumn: "playerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
