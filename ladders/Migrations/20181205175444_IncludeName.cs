using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class IncludeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProfileModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProfileModel");
        }
    }
}
