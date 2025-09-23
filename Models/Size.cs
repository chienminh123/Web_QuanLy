using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Size")]
    public class Size
    {
        [Key]
        public int MaSize { get; set; }
        [Required]
        public string TenSize { get; set; }
        [Required]
        public int SoLuongTon { get; set; }
        [Required]
        [ForeignKey("MaSanPham")]
        public SanPham SanPham { get; set; }
    }
}
