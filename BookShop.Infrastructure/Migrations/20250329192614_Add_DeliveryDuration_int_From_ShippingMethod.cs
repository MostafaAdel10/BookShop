using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_DeliveryDuration_int_From_ShippingMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estimated_Delivery_Time",
                table: "Shipping_Methods");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryDurationInDays",
                table: "Shipping_Methods",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDurationInDays",
                table: "Shipping_Methods");

            migrationBuilder.AddColumn<DateTime>(
                name: "Estimated_Delivery_Time",
                table: "Shipping_Methods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
