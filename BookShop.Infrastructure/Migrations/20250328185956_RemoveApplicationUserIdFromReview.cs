using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApplicationUserIdFromReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // حذف الفهرس (Index)
            migrationBuilder.DropIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews");

            // حذف المفتاح الأجنبي (Foreign Key)
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews");

            // حذف العمود بعد إزالة القيود
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // إعادة إضافة العمود في حالة التراجع
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // إعادة المفتاح الأجنبي
            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // إعادة الفهرس
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId");
        }
    }
}
