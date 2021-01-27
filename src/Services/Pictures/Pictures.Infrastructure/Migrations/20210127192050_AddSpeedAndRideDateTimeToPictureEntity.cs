using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pictures.Infrastructure.Migrations
{
    public partial class AddSpeedAndRideDateTimeToPictureEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RideDateTime",
                table: "Pictures",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<double>(
                name: "Speed",
                table: "Pictures",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RideDateTime",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Pictures");
        }
    }
}
