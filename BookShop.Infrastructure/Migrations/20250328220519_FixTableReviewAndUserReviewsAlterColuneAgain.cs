using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTableReviewAndUserReviewsAlterColuneAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Reviews_AspNetUsers_ApplicationUserId",
                table: "User_Reviews");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "User_Reviews",
                newName: "applicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Reviews_ApplicationUserId",
                table: "User_Reviews",
                newName: "IX_User_Reviews_applicationUserId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUser",
                table: "User_Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsAccpted",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Reviews_AspNetUsers_applicationUserId",
                table: "User_Reviews",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Reviews_AspNetUsers_applicationUserId",
                table: "User_Reviews");

            migrationBuilder.DropColumn(
                name: "ApplicationUser",
                table: "User_Reviews");

            migrationBuilder.DropColumn(
                name: "IsAccpted",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "User_Reviews",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Reviews_applicationUserId",
                table: "User_Reviews",
                newName: "IX_User_Reviews_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Reviews_AspNetUsers_ApplicationUserId",
                table: "User_Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
