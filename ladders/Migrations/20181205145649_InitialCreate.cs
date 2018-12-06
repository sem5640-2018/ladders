using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LadderModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LadderModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    Availability = table.Column<string>(nullable: false),
                    PreferredLocation = table.Column<string>(nullable: false),
                    Suspended = table.Column<bool>(nullable: false),
                    CurrentLadder = table.Column<int>(nullable: false),
                    LadderModelId = table.Column<int>(nullable: true),
                    LadderModelId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileModel_LadderModel_LadderModelId",
                        column: x => x.LadderModelId,
                        principalTable: "LadderModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileModel_LadderModel_LadderModelId1",
                        column: x => x.LadderModelId1,
                        principalTable: "LadderModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ranking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LadderModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ranking_LadderModel_LadderModelId",
                        column: x => x.LadderModelId,
                        principalTable: "LadderModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileModel_LadderModelId",
                table: "ProfileModel",
                column: "LadderModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileModel_LadderModelId1",
                table: "ProfileModel",
                column: "LadderModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ranking_LadderModelId",
                table: "Ranking",
                column: "LadderModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileModel");

            migrationBuilder.DropTable(
                name: "Ranking");

            migrationBuilder.DropTable(
                name: "LadderModel");
        }
    }
}
