using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ladders.Migrations
{
    public partial class StoreInternalBookingid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenge_Booking_BookingId",
                table: "Challenge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "bookingId",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Booking",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenge_Booking_BookingId",
                table: "Challenge",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenge_Booking_BookingId",
                table: "Challenge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "bookingId",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "bookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenge_Booking_BookingId",
                table: "Challenge",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "bookingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
