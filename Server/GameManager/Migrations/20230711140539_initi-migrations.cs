using Microsoft.EntityFrameworkCore.Migrations;

namespace GameManager.Migrations
{
    public partial class initimigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "matrix",
                table: "Board",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "matrix",
                table: "Board");
        }
    }
}
