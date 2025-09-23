using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using Web.PhanQuyen;

namespace Web.Repositories
{
    public class AdminRepo : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRepo(ApplicationDbContext context,
                        UserManager<User> userManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Các method admin role (mới thêm)
        public async Task<bool> IsAdminAsync(User user)
        {
            if (user == null) return false;
            return await _userManager.IsInRoleAsync(user, Roles.Admin.ToString());
        }

        public bool IsAdmin(User user)
        {
            if (user == null) return false;
            return _userManager.IsInRoleAsync(user, Roles.Admin.ToString()).Result;
        }

        public async Task AddAdminRoleAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Tạo role Admin nếu chưa tồn tại
                if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                }

                // Thêm user vào role Admin
                if (!await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }

        public async Task<List<User>> GetAllAdminsAsync()
        {
            var allUsers = _userManager.Users.ToList();
            var admins = new List<User>();

            foreach (var user in allUsers)
            {
                if (await IsAdminAsync(user))
                {
                    admins.Add(user);
                }
            }

            return admins;
        }

        public async Task RemoveAdminRoleAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.Admin.ToString());
            }
        }

        // Các method hiện có của bạn (giữ nguyên)
        public async Task<bool> AddProduct(AddProduct model)
        {
            try
            {
                // Kiểm tra TenTheLoai
                if (string.IsNullOrWhiteSpace(model.TenTheLoaiMoi))
                {
                    Console.WriteLine("TenTheLoai is null or empty");
                    return false;
                }

                // Kiểm tra và thêm thể loại nếu chưa tồn tại
                var existingTheLoai = await _context.TheLoais
                    .FirstOrDefaultAsync(t => t.TenTheLoai == model.TenTheLoaiMoi);
                if (existingTheLoai == null)
                {
                    existingTheLoai = new TheLoai { TenTheLoai = model.TenTheLoaiMoi };
                    _context.TheLoais.Add(existingTheLoai);
                    await _context.SaveChangesAsync(); // Lưu thể loại để lấy MaTheLoai
                }

                // Thêm sản phẩm
                var sanpham = new SanPham
                {
                    TenSanPham = model.TenSanPham,
                    Gia = model.Gia,
                    MoTa = model.MoTa,
                    MaTheLoai = existingTheLoai.MaTheLoai
                };
                _context.SanPhams.Add(sanpham);
                await _context.SaveChangesAsync(); // Lưu sản phẩm để lấy MaSanPham

                // Lưu tất cả thay đổi
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm sản phẩm: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<List<TheLoai>> GetTheLoais()
        {
            try
            {
                return await _context.TheLoais.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách thể loại: {ex.Message}");
                return new List<TheLoai>();
            }
        }
    }
}