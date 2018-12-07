using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class referenceladder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ranking_LadderModel_LadderModelId",
                table: "Ranking");

            migrationBuilder.AlterColumn<int>(
                name: "LadderModelId",
                table: "Ranking",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Draws",
                table: "Ranking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "Ranking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ranking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "Ranking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Ranking_LadderModel_LadderModelId",
                table: "Ranking",
                column: "LadderModelId",
                principalTable: "LadderModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ranking_LadderModel_LadderModelId",
                table: "Ranking");

            migrationBuilder.DropColumn(
                name: "Draws",
                table: "Ranking");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "Ranking");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ranking");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "Ranking");

            migrationBuilder.AlterColumn<int>(
                name: "LadderModelId",
                table: "Ranking",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Ranking_LadderModel_LadderModelId",
                table: "Ranking",
                column: "LadderModelId",
                principalTable: "LadderModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
