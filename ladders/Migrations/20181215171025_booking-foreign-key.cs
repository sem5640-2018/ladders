using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class bookingforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Facility_facilityId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "facilityId",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Facility_facilityId",
                table: "Booking",
                column: "facilityId",
                principalTable: "Facility",
                principalColumn: "facilityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Facility_facilityId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "facilityId",
                table: "Booking",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Facility_facilityId",
                table: "Booking",
                column: "facilityId",
                principalTable: "Facility",
                principalColumn: "facilityId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
