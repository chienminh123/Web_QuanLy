using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AddProduct
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [Display(Name = "Tên sản phẩm")]
        [MaxLength(100)]
        public string TenSanPham { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Display(Name = "Giá")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public double Gia { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [Display(Name = "Mô tả")]
        [MinLength(10, ErrorMessage = "Mô tả phải có ít nhất 10 ký tự")]
        public string MoTa { get; set; }


        [Display(Name = "Thể loại mới")]
        public string TenTheLoaiMoi { get; set; }

        // File uploads
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một hình ảnh")]
        [Display(Name = "Hình ảnh")]
        public List<IFormFile> HinhAnhs { get; set; } = new List<IFormFile>( );

        

        // Sizes configuration
        [Display(Name = "Kích thước và số lượng")]
        public List<SizeInput> Sizes { get; set; } = new List<SizeInput>
        {
            new SizeInput { TenSize = "S" },
            new SizeInput { TenSize = "M" },
            new SizeInput { TenSize = "L" },
            new SizeInput { TenSize = "XL" },
            new SizeInput { TenSize = "XXL" },
            new SizeInput { TenSize = "27" },
            new SizeInput { TenSize = "28" },
            new SizeInput { TenSize = "29" },
            new SizeInput { TenSize = "30" },
            new SizeInput { TenSize = "31" },
            new SizeInput { TenSize = "38" },
            new SizeInput { TenSize = "39" },
            new SizeInput { TenSize = "40" },
            new SizeInput { TenSize = "41" },
            new SizeInput { TenSize = "42" },
            new SizeInput { TenSize = "43" },
            new SizeInput { TenSize = "44" },
            new SizeInput { TenSize = "45" }

        };
    }

    public class SizeInput
    {
        public string TenSize { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int SoLuongTon { get; set; }
        
    }

    public class IFormFine
    {
        public int MaSanPham { get; set; }
        public string HinhAnhUrl { get; set; }
        public bool AnhBia { get; set; } = false;

    }
}
