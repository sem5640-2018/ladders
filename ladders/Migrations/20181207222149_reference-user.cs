using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class referenceuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ranking_UserId",
                table: "Ranking",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ranking_ProfileModel_UserId",
                table: "Ranking",
                column: "UserId",
                principalTable: "ProfileModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ranking_ProfileModel_UserId",
                table: "Ranking");

            migrationBuilder.DropIndex(
                name: "IX_Ranking_UserId",
                table: "Ranking");
        }
    }
}
