using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Web.Data;
using Web.PhanQuyen;

namespace Web.Areas.Identity.Pages.Account
{
    public class ConfirmCodeModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;


        public ConfirmCodeModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng nhập mã xác nhận.")]
            [Display(Name = "Mã xác nhận")]
            public string Code { get; set; }

            [Required]
            public string Email { get; set; }

            public string ReturnUrl { get; set; }
        }

        public IActionResult OnGet(string email, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Account/Register", new { area = "Identity" });
            }

            Input = new InputModel { Email = email, ReturnUrl = returnUrl };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"OnPostAsync called - Email: {Input.Email}, Code: {Input.Code}, ReturnUrl: {Input.ReturnUrl}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                Console.WriteLine("User not found");
                ModelState.AddModelError(string.Empty, "Email không tồn tại.");
                return Page();
            }

            if (user.ConfirmationCode == Input.Code && user.CodeGeneratedAt.HasValue &&
                (DateTime.UtcNow - user.CodeGeneratedAt.Value).TotalMinutes <= 30)
            {
                Console.WriteLine("Code is valid, updating user");
                user.EmailConfirmed = true;
                user.ConfirmationCode = null;
                user.CodeGeneratedAt = null;
                await _userManager.UpdateAsync(user);

                if (await _userManager.IsInRoleAsync(user, nameof(Roles.Admin)))
                {
                    return RedirectToPage("/Account/Admin", new { area = "Identity" });
                }

                Console.WriteLine("Redirecting to Login page");
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Input.ReturnUrl });
            }

            Console.WriteLine("Code is invalid or expired");
            ModelState.AddModelError(string.Empty, "Mã xác nhận không đúng hoặc đã hết hạn.");
            return Page();
        }

        public async Task<IActionResult> OnPostResendCodeAsync()
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return RedirectToPage("/Account/Register", new { area = "Identity" });
            }

            var code = new Random().Next(100000, 999999).ToString();
            user.ConfirmationCode = code;
            user.CodeGeneratedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var subject = "Xác nhận đăng ký (Gửi lại)";
            var message = $"<h3>Xác nhận đăng ký</h3><p>Mã xác nhận mới của bạn là: <strong>{code}</strong></p><p>Mã có hiệu lực trong 30 phút.</p>";
            await _emailSender.SendEmailAsync(Input.Email, subject, message);

            TempData["Message"] = "Mã xác nhận mới đã được gửi đến email của bạn.";
            return Page();
        }
    }
}
