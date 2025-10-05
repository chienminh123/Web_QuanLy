using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AddProduct
    {

        [Display(Name = "Tên sản phẩm")]
        [MaxLength(100)]
        public string TenSanPham { get; set; }

        [Display(Name = "Giá")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public double Gia { get; set; }

        [Display(Name = "Mô tả")]
        [MinLength(10, ErrorMessage = "Mô tả phải có ít nhất 10 ký tự")]
        public string MoTa { get; set; }


        [Display(Name = "Thể loại mới")]
        public string TenTheLoai { get; set; }
        public List<Size> Sizes { get; set; } = new List<Size>();
        [DataType(DataType.Upload)]
        public List<IFormFile> HinhAnhs { get; set; } = new List<IFormFile>();
    }
    
}
