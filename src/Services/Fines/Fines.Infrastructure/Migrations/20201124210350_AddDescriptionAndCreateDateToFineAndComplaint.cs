using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fines.Infrastructure.Migrations
{
    public partial class AddDescriptionAndCreateDateToFineAndComplaint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Fines",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Fines",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Complaints",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Fines");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Fines");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Complaints");
        }
    }
}
