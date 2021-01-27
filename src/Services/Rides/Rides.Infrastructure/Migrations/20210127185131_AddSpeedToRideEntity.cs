using Microsoft.EntityFrameworkCore.Migrations;

namespace Rides.Infrastructure.Migrations
{
    public partial class AddSpeedToRideEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Speed",
                table: "Rides",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Rides");
        }
    }
}
