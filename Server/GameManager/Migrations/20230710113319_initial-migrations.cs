using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameManager.Migrations
{
    public partial class initialmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "boardid",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_boardid",
                table: "Games",
                column: "boardid");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Board_boardid",
                table: "Games",
                column: "boardid",
                principalTable: "Board",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Board_boardid",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Board");

            migrationBuilder.DropIndex(
                name: "IX_Games_boardid",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "boardid",
                table: "Games");
        }
    }
}
