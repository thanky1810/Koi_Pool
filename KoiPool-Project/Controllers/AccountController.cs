using KoiPool_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiPool_Project.Models.ViewModels;
using System.Security.Claims;

namespace KoiPool_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
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
        public async Task<IActionResult> Login(LoginViewModel loginVM, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    // Retrieve the user info
                    var user = await _userManager.FindByNameAsync(loginVM.Username);

                    if (user != null)
                    {
                        // Check roles and redirect accordingly
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" }); // Redirect to Admin area
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home"); // Redirect to User's home page
                        }
                    }
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác");
            }

            // Return the view with the login model
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
                    // Check if email already exists
                    var existingUser = await _userManager.FindByEmailAsync(user.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email này đã được sử dụng");
                        return View(user);
                    }

                    // Check if username already exists
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
                        Occupation = "User" // Default role or occupation
                    };

                    var result = await _userManager.CreateAsync(newUser, user.Password);

                    if (result.Succeeded)
                    {
                        // Assign default role (e.g., "User")
                        await _userManager.AddToRoleAsync(newUser, "User");

                        // Sign in the user after successful registration
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
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký: " + ex.Message);
                }
            }

            return View(user);
        }

        // Method to test database connection
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
            return Redirect(returnUrl);
        }
    }
}