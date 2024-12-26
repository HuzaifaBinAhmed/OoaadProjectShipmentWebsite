using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebsiteApi.Migrations
{
    /// <inheritdoc />
    public partial class addShiperAdvice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shiperAdvices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipmentId = table.Column<int>(type: "int", nullable: false),
                    AdviceMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shiperAdvices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shiperAdvices_shipments_shipmentId",
                        column: x => x.shipmentId,
                        principalTable: "shipments",
                        principalColumn: "ShipmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shiperAdvices_shipmentId",
                table: "shiperAdvices",
                column: "shipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shiperAdvices");
        }
    }
}
