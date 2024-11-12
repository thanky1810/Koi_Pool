using KoiPool_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiPool_Project.Models.ViewModels;
namespace KoiPool_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private readonly DataContext _context;

        public AccountController(
            UserManager<AppUserModel> userManager,
            SignInManager<AppUserModel> signInManager,
            DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result =
                    await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    //return !string.IsNullOrEmpty(loginVM.ReturnUrl)
                    //    ? Redirect(loginVM.ReturnUrl)
                    //    : RedirectToAction("Index", "Home");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác");
            }
            return View(loginVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem email đã tồn tại chưa
                    var existingUser = await _userManager.FindByEmailAsync(user.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email này đã được sử dụng");
                        return View(user);
                    }

                    // Kiểm tra username đã tồn tại chưa
                    var existingUsername = await _userManager.FindByNameAsync(user.Username);
                    if (existingUsername != null)
                    {
                        ModelState.AddModelError("Username", "Tên đăng nhập này đã được sử dụng");
                        return View(user);
                    }

                    var newUser = new AppUserModel
                    {
                        UserName = user.Username,
                        Email = user.Email,
                        Occupation = "User" // Giá trị mặc định
                    };

                    var result = await _userManager.CreateAsync(newUser, user.Password);

                    if (result.Succeeded)
                    {
                        // Đăng nhập người dùng sau khi đăng ký thành công
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký: " + ex.Message);
                }
            }

            // Trả về view với các lỗi nếu có
            return View(user);
        }
        // Phương thức kiểm tra kết nối database
        public IActionResult TestDbConnection()
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                return Json(new { success = canConnect });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", returnUrl);
        }
    }
}
