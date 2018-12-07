using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class resolvereferenceerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileModel_LadderModel_LadderModelId",
                table: "ProfileModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileModel_LadderModel_LadderModelId1",
                table: "ProfileModel");

            migrationBuilder.DropColumn(
                name: "CurrentLadder",
                table: "ProfileModel");

            migrationBuilder.RenameColumn(
                name: "LadderModelId1",
                table: "ProfileModel",
                newName: "CurrentLadderId");

            migrationBuilder.RenameColumn(
                name: "LadderModelId",
                table: "ProfileModel",
                newName: "ApprovalLadderId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileModel_LadderModelId1",
                table: "ProfileModel",
                newName: "IX_ProfileModel_CurrentLadderId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileModel_LadderModelId",
                table: "ProfileModel",
                newName: "IX_ProfileModel_ApprovalLadderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileModel_LadderModel_ApprovalLadderId",
                table: "ProfileModel",
                column: "ApprovalLadderId",
                principalTable: "LadderModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileModel_LadderModel_CurrentLadderId",
                table: "ProfileModel",
                column: "CurrentLadderId",
                principalTable: "LadderModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileModel_LadderModel_ApprovalLadderId",
                table: "ProfileModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileModel_LadderModel_CurrentLadderId",
                table: "ProfileModel");

            migrationBuilder.RenameColumn(
                name: "CurrentLadderId",
                table: "ProfileModel",
                newName: "LadderModelId1");

            migrationBuilder.RenameColumn(
                name: "ApprovalLadderId",
                table: "ProfileModel",
                newName: "LadderModelId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileModel_CurrentLadderId",
                table: "ProfileModel",
                newName: "IX_ProfileModel_LadderModelId1");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileModel_ApprovalLadderId",
                table: "ProfileModel",
                newName: "IX_ProfileModel_LadderModelId");

            migrationBuilder.AddColumn<int>(
                name: "CurrentLadder",
                table: "ProfileModel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileModel_LadderModel_LadderModelId",
                table: "ProfileModel",
                column: "LadderModelId",
                principalTable: "LadderModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileModel_LadderModel_LadderModelId1",
                table: "ProfileModel",
                column: "LadderModelId1",
                principalTable: "LadderModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
