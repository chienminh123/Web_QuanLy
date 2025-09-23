using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("HinhAnh")]
    public class HinhAnh
    {
        [Key]
        public int HinhAnhId { get; set; }
        [Required]
        public int MaSanPham { get; set; }
        [Required]
        public string HinhAnhUrl { get; set; }
        [Required]
        public bool AnhBia { get; set; } = false;
        [ForeignKey("MaSanPham")]
        public SanPham SanPham { get; set; }
    }
}
