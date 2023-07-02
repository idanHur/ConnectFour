using Microsoft.EntityFrameworkCore.Migrations;

namespace GameManager.Migrations
{
    public partial class PlayerClassUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "phoneNumber",
                table: "Players",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Players",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "phoneNumber",
                table: "Players",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
