using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Web.Data
{
    public class User:IdentityUser
    {
        [Required]
        public string HoTen { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required]
        public DateTime NgaySinh { get; set; }
        public float TichDiem { get; set; }
        public string ConfirmationCode { get; set; } // Lưu mã xác nhận
        public DateTime? CodeGeneratedAt { get; set; } // Thời gian tạo mã
    }
}
