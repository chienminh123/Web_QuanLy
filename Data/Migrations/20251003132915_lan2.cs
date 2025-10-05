using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    public partial class lan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Size_SanPham_MaSanPham",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_MaSanPham",
                table: "Size");

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "Size",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Size_SanPhamMaSanPham",
                table: "Size",
                column: "SanPhamMaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_Size_SanPham_SanPhamMaSanPham",
                table: "Size",
                column: "SanPhamMaSanPham",
                principalTable: "SanPham",
                principalColumn: "MaSanPham");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Size_SanPham_SanPhamMaSanPham",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_SanPhamMaSanPham",
                table: "Size");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_Size_MaSanPham",
                table: "Size",
                column: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_Size_SanPham_MaSanPham",
                table: "Size",
                column: "MaSanPham",
                principalTable: "SanPham",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
