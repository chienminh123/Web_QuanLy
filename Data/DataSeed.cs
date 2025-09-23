using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Web.PhanQuyen;

namespace Web.Data
{
    public class DataSeed
    {
        public static async Task KhoiTaoDL(IServiceProvider dichVu)
        {
            var quanlyNguoidung = dichVu.GetRequiredService<UserManager<User>>();
            var quanlyVaitro = dichVu.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = dichVu.GetRequiredService<ILogger<DataSeed>>();

            // Kiểm tra role trước khi tạo
            if (!await quanlyVaitro.RoleExistsAsync(Roles.Admin.ToString()))
                await quanlyVaitro.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            if (!await quanlyVaitro.RoleExistsAsync(Roles.User.ToString()))
                await quanlyVaitro.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Tạo user Admin
            var adminEmail = "admin@Adidas.com";
            var nguoidungtrongCSDL = await quanlyNguoidung.FindByEmailAsync(adminEmail);

            if (nguoidungtrongCSDL == null)
            {
                logger.LogInformation("Tạo tài khoản Admin {Email}...", adminEmail);
                var quantri = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    ConfirmationCode = "000000",
                    DiaChi = "dia chi",
                    HoTen = "admin",
                    TichDiem = 0,
                   
                };

                var ketqua = await quanlyNguoidung.CreateAsync(quantri, "Admin@12345");
                if (ketqua.Succeeded)
                {
                    logger.LogInformation("Tạo tài khoản Admin thành công.");
                    await quanlyNguoidung.AddToRoleAsync(quantri, Roles.Admin.ToString());
                    logger.LogInformation("Gán vai trò Admin thành công.");
                }
                else
                {
                    logger.LogError("Lỗi tạo tài khoản Admin: ");
                    foreach (var error in ketqua.Errors)
                    {
                        logger.LogError("Lỗi: {Description}", error.Description);
                    }
                }
            }
            else
            {
                logger.LogInformation("Tài khoản Admin {Email} đã tồn tại.", adminEmail);
                // Đảm bảo user có role Admin
                if (!await quanlyNguoidung.IsInRoleAsync(nguoidungtrongCSDL, Roles.Admin.ToString()))
                {
                    await quanlyNguoidung.AddToRoleAsync(nguoidungtrongCSDL, Roles.Admin.ToString());
                    logger.LogInformation("Đã gán lại vai trò Admin cho {Email}", adminEmail);
                }
            }
        }
    }
}