using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class Storeuserposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Ranking",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Ranking");
        }
    }
}
