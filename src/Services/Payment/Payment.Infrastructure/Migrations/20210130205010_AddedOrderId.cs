using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Infrastructure.Migrations
{
    public partial class AddedOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payments");
        }
    }
}
