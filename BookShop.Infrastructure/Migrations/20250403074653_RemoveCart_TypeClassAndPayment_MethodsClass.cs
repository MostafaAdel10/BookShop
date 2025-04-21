using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCart_TypeClassAndPayment_MethodsClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Payment_Methods_Payment_MethodsID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payment_Methods_PaymentMethodsID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Payment_Methods");

            migrationBuilder.DropTable(
                name: "Card_Types");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentMethodsID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Payment_MethodsID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PaymentMethodsID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Payment_MethodsID",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodsID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Payment_MethodsID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Card_Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment_Methods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Card_TypeId = table.Column<int>(type: "int", nullable: true),
                    Card_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Card_TypeId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Expiration_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Is_Default = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment_Methods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Methods_Card_Types_Card_TypeId",
                        column: x => x.Card_TypeId,
                        principalTable: "Card_Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_Methods_Card_Types_Card_TypeId1",
                        column: x => x.Card_TypeId1,
                        principalTable: "Card_Types",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodsID",
                table: "Orders",
                column: "PaymentMethodsID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Payment_MethodsID",
                table: "AspNetUsers",
                column: "Payment_MethodsID",
                unique: true,
                filter: "[Payment_MethodsID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Methods_Card_TypeId",
                table: "Payment_Methods",
                column: "Card_TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Methods_Card_TypeId1",
                table: "Payment_Methods",
                column: "Card_TypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Payment_Methods_Payment_MethodsID",
                table: "AspNetUsers",
                column: "Payment_MethodsID",
                principalTable: "Payment_Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payment_Methods_PaymentMethodsID",
                table: "Orders",
                column: "PaymentMethodsID",
                principalTable: "Payment_Methods",
                principalColumn: "Id");
        }
    }
}
