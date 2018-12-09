using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class Removeuserlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileModel_LadderModel_CurrentLadderId",
                table: "ProfileModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Ranking_ProfileModel_UserId",
                table: "Ranking");

            migrationBuilder.DropIndex(
                name: "IX_Ranking_UserId",
                table: "Ranking");

            migrationBuilder.DropIndex(
                name: "IX_ProfileModel_CurrentLadderId",
                table: "ProfileModel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ranking");

            migrationBuilder.RenameColumn(
                name: "CurrentLadderId",
                table: "ProfileModel",
                newName: "CurrentRankingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileModel_CurrentRankingId",
                table: "ProfileModel",
                column: "CurrentRankingId",
                unique: true,
                filter: "[CurrentRankingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileModel_Ranking_CurrentRankingId",
                table: "ProfileModel",
                column: "CurrentRankingId",
                principalTable: "Ranking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileModel_Ranking_CurrentRankingId",
                table: "ProfileModel");

            migrationBuilder.DropIndex(
                name: "IX_ProfileModel_CurrentRankingId",
                table: "ProfileModel");

            migrationBuilder.RenameColumn(
                name: "CurrentRankingId",
                table: "ProfileModel",
                newName: "CurrentLadderId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ranking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ranking_UserId",
                table: "Ranking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileModel_CurrentLadderId",
                table: "ProfileModel",
                column: "CurrentLadderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileModel_LadderModel_CurrentLadderId",
                table: "ProfileModel",
                column: "CurrentLadderId",
                principalTable: "LadderModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ranking_ProfileModel_UserId",
                table: "Ranking",
                column: "UserId",
                principalTable: "ProfileModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
