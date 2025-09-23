using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }
        [Required]
        public int  MaTheLoai { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenSanPham { get; set; }
        [Required]
        public double Gia { get; set; }
        [Required]
        public string MoTa { get; set; }
        [ForeignKey("MaTheLoai")]
        public TheLoai TheLoai { get; set; }
        public List<HinhAnh> HinhAnhs { get; set; } = new List<HinhAnh>();
        public ICollection<Size> Sizes { get; set; }

    }


}