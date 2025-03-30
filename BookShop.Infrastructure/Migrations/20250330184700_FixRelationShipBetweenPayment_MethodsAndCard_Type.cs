using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationShipBetweenPayment_MethodsAndCard_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Card_TypeId1",
                table: "Payment_Methods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Methods_Card_TypeId1",
                table: "Payment_Methods",
                column: "Card_TypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Methods_Card_Types_Card_TypeId1",
                table: "Payment_Methods",
                column: "Card_TypeId1",
                principalTable: "Card_Types",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Methods_Card_Types_Card_TypeId1",
                table: "Payment_Methods");

            migrationBuilder.DropIndex(
                name: "IX_Payment_Methods_Card_TypeId1",
                table: "Payment_Methods");

            migrationBuilder.DropColumn(
                name: "Card_TypeId1",
                table: "Payment_Methods");
        }
    }
}
