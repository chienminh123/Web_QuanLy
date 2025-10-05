using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<HinhAnh> HinhAnhs { get; set; }
        public DbSet<Size> Sizes { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<SanPham>()
        //        .HasMany(s => s.HinhAnhs)
        //        .WithOne(h => h.SanPham)
        //        .HasForeignKey(h => h.MaSanPham)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<SanPham>()
        //        .HasMany(s => s.Sizes)
        //        .WithOne(sz => sz.SanPham)
        //        .HasForeignKey(sz => sz.SanPham)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<SanPham>()
        //        .HasOne(s => s.TheLoai)
        //        .WithMany(t => t.SanPham)
        //        .HasForeignKey(s => s.MaTheLoai);
        //}
    }
}
