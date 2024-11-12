using KoiPool_Project.Models.ViewModels;
using KoiPool_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace KoiPool_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppRolesController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppRolesController(
            UserManager<AppUserModel> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        private async Task EnsureRolesCreated()
        {
            string[] roles = { "Admin", "User", "Manager" }; // Thêm các roles mặc định
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // GET: Hiển thị form tạo tài khoản
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Đảm bảo roles đã được tạo
            await EnsureRolesCreated();

            var model = new CreateUserViewModel
            {
                Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList()
            };

            return View(model);
        }

        // POST: Xử lý tạo tài khoản và gán vai trò
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra email đã tồn tại
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    model.Roles = _roleManager.Roles.Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList();
                    return View(model);
                }

                // Tạo người dùng mới
                var newUser = new AppUserModel
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Occupation = model.Role,
                    EmailConfirmed = true // Tự động xác nhận email
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    // Đảm bảo role tồn tại
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    // Gán vai trò cho người dùng
                    var roleResult = await _userManager.AddToRoleAsync(newUser, model.Role);
                    if (roleResult.Succeeded)
                    {
                        TempData["Success"] = "Tạo tài khoản và gán vai trò thành công!";
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    // Nếu gán vai trò thất bại
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    // Xóa user nếu không gán được role
                    await _userManager.DeleteAsync(newUser);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // Nếu có lỗi, load lại danh sách roles
            model.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return View(model);
        }
    }
}