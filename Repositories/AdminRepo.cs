using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;
using Web.PhanQuyen;
using Web.Data;
using Web.Models;
using Web.PhanQuyen;
using Web.Repositories;

namespace Web.Repositories
{
    public class AdminRepo : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminRepo> _logger;
        public AdminRepo(ApplicationDbContext context,
                        UserManager<User> userManager,
                        RoleManager<IdentityRole> roleManager,
                        ILogger<AdminRepo> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
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
            return _userManager.IsInRoleAsync(user, Roles.Admin.ToString()).GetAwaiter().GetResult();
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
            var adminRole = await _roleManager.FindByNameAsync(Roles.Admin.ToString());
            if (adminRole == null) return new List<User>();

            var adminIds = await _context.UserRoles
                .Where(ur => ur.RoleId == adminRole.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            return await _userManager.Users
                .Where(u => adminIds.Contains(u.Id))
                .ToListAsync();
        }

        public async Task RemoveAdminRoleAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.Admin.ToString());
            }
        }
        public async Task<bool> AddProductAsync(AddProduct model, IWebHostEnvironment environment)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Bắt đầu thêm sản phẩm: {TenSanPham}", model.TenSanPham);
                if (string.IsNullOrWhiteSpace(model.TenTheLoai))
                {
                    _logger.LogWarning("TenTheLoai trống, hủy giao dịch.");
                    return false;
                }

                var theLoai = await _context.TheLoais.FirstOrDefaultAsync(t => t.TenTheLoai == model.TenTheLoai);
                if (theLoai == null)
                {
                    theLoai = new TheLoai { TenTheLoai = model.TenTheLoai };
                    _context.TheLoais.Add(theLoai);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Thêm thể loại mới: {TenTheLoai}", model.TenTheLoai);
                }

                var sanPham = await _context.SanPhams.FirstOrDefaultAsync(t => t.TenSanPham == model.TenSanPham);
                if (sanPham == null)
                {
                    //chưa có kiểm tra tên sp đã nhập r 
                    sanPham = new SanPham
                    {
                        TenSanPham = model.TenSanPham,
                        Gia = model.Gia,
                        MoTa = model.MoTa,
                        MaTheLoai = theLoai.MaTheLoai,
                        Sizes = new List<Size>(),
                        HinhAnhs = new List<HinhAnh>()
                    };
                    _context.SanPhams.Add(sanPham);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Thêm sản phẩm thành công, MaSanPham: {MaSanPham}", sanPham.MaSanPham);
                }
                // Xử lý size
                if (model.Sizes == null || !model.Sizes.Any(s => !string.IsNullOrEmpty(s.TenSize)))
                {
                    _logger.LogWarning("Danh sách size không hợp lệ, hủy giao dịch.");
                    await transaction.RollbackAsync();
                    return false;
                }
                foreach (var size in model.Sizes)
                {
                    if (!string.IsNullOrEmpty(size.TenSize))
                    {
                        var newSize = new Size
                        {
                            TenSize = size.TenSize,
                            SoLuongTon = size.SoLuongTon,
                            MaSanPham = sanPham.MaSanPham
                        };
                        _context.Sizes.Add(newSize);
                        sanPham.Sizes.Add(newSize);
                    }
                }

                // Xử lý hình ảnh
                _logger.LogInformation("Số lượng file HinhAnhs: {Count}", model.HinhAnhs?.Count ?? 0);
                if (model.HinhAnhs != null && model.HinhAnhs.Any(f => f != null && f.Length > 0))
                {
                    for (int i = 0; i < model.HinhAnhs.Count; i++)
                    {
                        var file = model.HinhAnhs[i];
                        if (file != null && file.Length > 0)
                        {
                            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                            {
                                _logger.LogWarning("Định dạng file không hợp lệ: {Extension}", extension);
                                await transaction.RollbackAsync();
                                return false;
                            }
                            if (file.Length > 5 * 1024 * 1024)
                            {
                                _logger.LogWarning("Kích thước file quá lớn: {Length}", file.Length);
                                await transaction.RollbackAsync();
                                return false;
                            }
                            var uploadsFolder = Path.Combine(environment.WebRootPath, "img/Server");
                            Directory.CreateDirectory(uploadsFolder);
                            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            var hinhAnh = new HinhAnh
                            {
                                HinhAnhUrl = "/img/Server/" + uniqueFileName,
                                AnhBia = (i == 0),
                                MaSanPham = sanPham.MaSanPham
                            };
                            _context.HinhAnhs.Add(hinhAnh);
                            sanPham.HinhAnhs.Add(hinhAnh);
                            _logger.LogInformation("Đã lưu hình ảnh: {HinhAnhUrl}", hinhAnh.HinhAnhUrl);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Không có hình ảnh nào được gửi lên.");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("Giao dịch hoàn tất thành công.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm: {Message}", ex.Message);
                await transaction.RollbackAsync();
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