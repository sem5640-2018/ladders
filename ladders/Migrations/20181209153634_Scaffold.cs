using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class Scaffold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LadderModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LadderModel", x => x.Id);
                });

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
                name: "ProfileModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    Availability = table.Column<string>(nullable: false),
                    PreferredLocation = table.Column<string>(nullable: false),
                    Suspended = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CurrentLadderId = table.Column<int>(nullable: true),
                    ApprovalLadderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileModel_LadderModel_ApprovalLadderId",
                        column: x => x.ApprovalLadderId,
                        principalTable: "LadderModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileModel_LadderModel_CurrentLadderId",
                        column: x => x.CurrentLadderId,
                        principalTable: "LadderModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    facilityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    facilityName = table.Column<string>(nullable: true),
                    isBlock = table.Column<bool>(nullable: false),
                    venueId = table.Column<int>(nullable: false),
                    sportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.facilityId);
                    table.ForeignKey(
                        name: "FK_Facility_Sport_sportId",
                        column: x => x.sportId,
                        principalTable: "Sport",
                        principalColumn: "sportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facility_Venue_venueId",
                        column: x => x.venueId,
                        principalTable: "Venue",
                        principalColumn: "venueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ranking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Wins = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false),
                    Draws = table.Column<int>(nullable: false),
                    LadderModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ranking_LadderModel_LadderModelId",
                        column: x => x.LadderModelId,
                        principalTable: "LadderModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ranking_ProfileModel_UserId",
                        column: x => x.UserId,
                        principalTable: "ProfileModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    bookingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bookingDateTime = table.Column<DateTime>(nullable: false),
                    userId = table.Column<string>(nullable: true),
                    facilityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.bookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Facility_facilityId",
                        column: x => x.facilityId,
                        principalTable: "Facility",
                        principalColumn: "facilityId",
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
                    ChallengedTime = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Resolved = table.Column<bool>(nullable: false),
                    Result = table.Column<int>(nullable: false),
                    RankingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenge_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "bookingId",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Challenge_Ranking_RankingId",
                        column: x => x.RankingId,
                        principalTable: "Ranking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_facilityId",
                table: "Booking",
                column: "facilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_BookingId",
                table: "Challenge",
                column: "BookingId");

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
                name: "IX_Challenge_RankingId",
                table: "Challenge",
                column: "RankingId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_sportId",
                table: "Facility",
                column: "sportId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_venueId",
                table: "Facility",
                column: "venueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileModel_ApprovalLadderId",
                table: "ProfileModel",
                column: "ApprovalLadderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileModel_CurrentLadderId",
                table: "ProfileModel",
                column: "CurrentLadderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ranking_LadderModelId",
                table: "Ranking",
                column: "LadderModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Ranking_UserId",
                table: "Ranking",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Challenge");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Ranking");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "ProfileModel");

            migrationBuilder.DropTable(
                name: "Sport");

            migrationBuilder.DropTable(
                name: "Venue");

            migrationBuilder.DropTable(
                name: "LadderModel");
        }
    }
}
