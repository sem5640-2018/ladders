using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class LinkChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankingId",
                table: "Challenge",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_RankingId",
                table: "Challenge",
                column: "RankingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenge_Ranking_RankingId",
                table: "Challenge",
                column: "RankingId",
                principalTable: "Ranking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenge_Ranking_RankingId",
                table: "Challenge");

            migrationBuilder.DropIndex(
                name: "IX_Challenge_RankingId",
                table: "Challenge");

            migrationBuilder.DropColumn(
                name: "RankingId",
                table: "Challenge");
        }
    }
}
