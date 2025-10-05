using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    public partial class lan3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnh_SanPham_MaSanPham",
                table: "HinhAnh");

            migrationBuilder.DropIndex(
                name: "IX_HinhAnh_MaSanPham",
                table: "HinhAnh");

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "HinhAnh",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnh_SanPhamMaSanPham",
                table: "HinhAnh",
                column: "SanPhamMaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnh_SanPham_SanPhamMaSanPham",
                table: "HinhAnh",
                column: "SanPhamMaSanPham",
                principalTable: "SanPham",
                principalColumn: "MaSanPham");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnh_SanPham_SanPhamMaSanPham",
                table: "HinhAnh");

            migrationBuilder.DropIndex(
                name: "IX_HinhAnh_SanPhamMaSanPham",
                table: "HinhAnh");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "HinhAnh");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnh_MaSanPham",
                table: "HinhAnh",
                column: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnh_SanPham_MaSanPham",
                table: "HinhAnh",
                column: "MaSanPham",
                principalTable: "SanPham",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
