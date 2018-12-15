using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class optionalkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facility_Sport_sportId",
                table: "Facility");

            migrationBuilder.DropForeignKey(
                name: "FK_Facility_Venue_venueId",
                table: "Facility");

            migrationBuilder.AlterColumn<int>(
                name: "venueId",
                table: "Facility",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "sportId",
                table: "Facility",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Facility_Sport_sportId",
                table: "Facility",
                column: "sportId",
                principalTable: "Sport",
                principalColumn: "sportId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facility_Venue_venueId",
                table: "Facility",
                column: "venueId",
                principalTable: "Venue",
                principalColumn: "venueId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facility_Sport_sportId",
                table: "Facility");

            migrationBuilder.DropForeignKey(
                name: "FK_Facility_Venue_venueId",
                table: "Facility");

            migrationBuilder.AlterColumn<int>(
                name: "venueId",
                table: "Facility",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sportId",
                table: "Facility",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Facility_Sport_sportId",
                table: "Facility",
                column: "sportId",
                principalTable: "Sport",
                principalColumn: "sportId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facility_Venue_venueId",
                table: "Facility",
                column: "venueId",
                principalTable: "Venue",
                principalColumn: "venueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
