using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Web.Data;
using Web.Models;
using Web.PhanQuyen;
using Web.Repositories;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class AdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _environment; // Để xử lý upload file
        private readonly ApplicationDbContext _context; // Thêm DbContext để sử dụng trực tiếp nếu cần

        public AdminController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IAdminRepository adminRepository,
            IWebHostEnvironment environment,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _adminRepository = adminRepository;
            _environment = environment;
            _context = context;
        }

        [HttpGet]
        public IActionResult AdminLogin(string? returnUrl = null)
        {
            // Nếu đã đăng nhập và có quyền admin thì chuyển hướng đến dashboard
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (_adminRepository.IsAdmin(user))
                {
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    // Nếu không phải admin, đăng xuất và hiển thị thông báo
                    _signInManager.SignOutAsync().Wait();
                    ModelState.AddModelError(string.Empty, "Bạn không có quyền truy cập trang admin.");
                }
            }

            // Lưu returnUrl để chuyển hướng sau khi đăng nhập thành công
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(LoginViewModel model, string? returnUrl = null)
        {
            // Nếu đã đăng nhập, chuyển hướng đến dashboard
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra xem email có phải admin không trước khi đăng nhập
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _adminRepository.IsAdminAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Email này không có quyền truy cập trang admin.");
                    return View(model);
                }

                // Thực hiện đăng nhập
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    
                    await _userManager.UpdateAsync(user);

                    // Chuyển hướng theo returnUrl hoặc dashboard mặc định
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard");
                    }
                }

                // Xử lý các trường hợp đăng nhập thất bại
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa tạm thời.");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
                    return View(model);
                }
            }

            // Nếu model không hợp lệ, trả lại view với dữ liệu đã nhập
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.TheLoais = await _adminRepository.GetTheLoais();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProduct model)
        {

            if (ModelState.IsValid)
            {
                var success = await _adminRepository.AddProductAsync(model, _environment);
                if (success)
                {
                    Console.WriteLine("Sản phẩm được thêm thành công");
                    return RedirectToAction("AddProduct");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi khi thêm sản phẩm. Vui lòng kiểm tra log để biết chi tiết.");
                }
            }
            else
            {
                Console.WriteLine("ModelState is invalid");
            }


            ViewBag.TheLoais = await _adminRepository.GetTheLoais();
            return View(model);
        }



    }
}