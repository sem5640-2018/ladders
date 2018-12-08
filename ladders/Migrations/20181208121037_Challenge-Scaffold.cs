using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class ChallengeScaffold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sport",
                columns: table => new
                {
                    sportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sportName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sport", x => x.sportId);
                });

            migrationBuilder.CreateTable(
                name: "Venue",
                columns: table => new
                {
                    venueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    venueName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venue", x => x.venueId);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    facilityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    facilityName = table.Column<string>(nullable: true),
                    isBlock = table.Column<bool>(nullable: false),
                    venueId = table.Column<int>(nullable: true),
                    sportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.facilityId);
                    table.ForeignKey(
                        name: "FK_Facility_Sport_sportId",
                        column: x => x.sportId,
                        principalTable: "Sport",
                        principalColumn: "sportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facility_Venue_venueId",
                        column: x => x.venueId,
                        principalTable: "Venue",
                        principalColumn: "venueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Challenge",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChallengerId = table.Column<int>(nullable: true),
                    ChallengeeId = table.Column<int>(nullable: true),
                    LadderId = table.Column<int>(nullable: true),
                    BookingId = table.Column<int>(nullable: false),
                    facilityId = table.Column<int>(nullable: true),
                    ChallengedTime = table.Column<DateTime>(nullable: false),
                    Resolved = table.Column<bool>(nullable: false),
                    Result = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenge_ProfileModel_ChallengeeId",
                        column: x => x.ChallengeeId,
                        principalTable: "ProfileModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenge_ProfileModel_ChallengerId",
                        column: x => x.ChallengerId,
                        principalTable: "ProfileModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenge_LadderModel_LadderId",
                        column: x => x.LadderId,
                        principalTable: "LadderModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenge_Facility_facilityId",
                        column: x => x.facilityId,
                        principalTable: "Facility",
                        principalColumn: "facilityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_ChallengeeId",
                table: "Challenge",
                column: "ChallengeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_ChallengerId",
                table: "Challenge",
                column: "ChallengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_LadderId",
                table: "Challenge",
                column: "LadderId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_facilityId",
                table: "Challenge",
                column: "facilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_sportId",
                table: "Facility",
                column: "sportId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_venueId",
                table: "Facility",
                column: "venueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Challenge");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "Sport");

            migrationBuilder.DropTable(
                name: "Venue");
        }
    }
}
