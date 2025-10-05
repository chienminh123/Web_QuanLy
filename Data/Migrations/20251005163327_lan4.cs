using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    public partial class lan4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnh_SanPham_SanPhamMaSanPham",
                table: "HinhAnh");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_SanPham_SanPhamMaSanPham",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_SanPhamMaSanPham",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_HinhAnh_SanPhamMaSanPham",
                table: "HinhAnh");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "Size");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "HinhAnh");

            migrationBuilder.CreateIndex(
                name: "IX_Size_MaSanPham",
                table: "Size",
                column: "MaSanPham");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Size_SanPham_MaSanPham",
                table: "Size",
                column: "MaSanPham",
                principalTable: "SanPham",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnh_SanPham_MaSanPham",
                table: "HinhAnh");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_SanPham_MaSanPham",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_MaSanPham",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_HinhAnh_MaSanPham",
                table: "HinhAnh");

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "Size",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "HinhAnh",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Size_SanPhamMaSanPham",
                table: "Size",
                column: "SanPhamMaSanPham");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Size_SanPham_SanPhamMaSanPham",
                table: "Size",
                column: "SanPhamMaSanPham",
                principalTable: "SanPham",
                principalColumn: "MaSanPham");
        }
    }
}
