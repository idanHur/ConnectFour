using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameManager.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    playerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    playerName = table.Column<string>(nullable: false),
                    phoneNumber = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.playerId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    gameStatus = table.Column<int>(nullable: false),
                    currentPlayer = table.Column<int>(nullable: false),
                    startTime = table.Column<DateTime>(nullable: false),
                    gameDuration = table.Column<TimeSpan>(nullable: false),
                    gameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    playerId = table.Column<int>(nullable: false),
                    playerId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.gameId);
                    table.ForeignKey(
                        name: "FK_Games_Players_playerId",
                        column: x => x.playerId,
                        principalTable: "Players",
                        principalColumn: "playerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Players_playerId1",
                        column: x => x.playerId1,
                        principalTable: "Players",
                        principalColumn: "playerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Move",
                columns: table => new
                {
                    columnNumber = table.Column<int>(nullable: false),
                    Player = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    gameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Move", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Move_Games_gameId",
                        column: x => x.gameId,
                        principalTable: "Games",
                        principalColumn: "gameId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_playerId",
                table: "Games",
                column: "playerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_playerId1",
                table: "Games",
                column: "playerId1",
                unique: true,
                filter: "[playerId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Move_gameId",
                table: "Move",
                column: "gameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Move");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
