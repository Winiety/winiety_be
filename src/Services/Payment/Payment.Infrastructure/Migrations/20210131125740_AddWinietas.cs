using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Infrastructure.Migrations
{
    public partial class AddWinietas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Winietas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<string>(nullable: true),
                    PayuUrl = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    Cost = table.Column<double>(nullable: false),
                    ExpirationDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winietas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Winietas");
        }
    }
}
