using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiPool_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToAspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột 'sex' vào bảng 'AspNetUsers'
            migrationBuilder.AddColumn<string>(
                name: "sex",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa cột 'sex' khỏi bảng 'AspNetUsers' khi rollback
            migrationBuilder.DropColumn(
                name: "sex",
                table: "AspNetUsers");
        }
    }
}
