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
    }
}
