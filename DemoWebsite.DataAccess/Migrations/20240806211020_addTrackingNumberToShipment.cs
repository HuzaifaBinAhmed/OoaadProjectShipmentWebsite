using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebsiteApi.Migrations
{
    /// <inheritdoc />
    public partial class addTrackingNumberToShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TrackingNumber",
                table: "shipments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "shipments");
        }
    }
}
