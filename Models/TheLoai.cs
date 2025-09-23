using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("TheLoai")]
    public class TheLoai
    {
        [Key]
        public int MaTheLoai { get; set; }
        [Required]
        public string TenTheLoai { get; set; }

        public ICollection<SanPham> SanPham { get; set; }
    }
}
