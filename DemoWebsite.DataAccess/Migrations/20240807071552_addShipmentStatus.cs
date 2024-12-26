using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebsiteApi.Migrations
{
    /// <inheritdoc />
    public partial class addShipmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipmentStatus",
                table: "shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipmentStatus",
                table: "shipments");
        }
    }
}
