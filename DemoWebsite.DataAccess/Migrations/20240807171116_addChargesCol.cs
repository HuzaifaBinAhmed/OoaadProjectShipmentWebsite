using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebsiteApi.Migrations
{
    /// <inheritdoc />
    public partial class addChargesCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CompanyCharges",
                table: "shipments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyCharges",
                table: "shipments");
        }
    }
}
