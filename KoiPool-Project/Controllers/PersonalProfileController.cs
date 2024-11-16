using KoiPool_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace KoiPool_Project.Controllers
{
    public class PersonalProfileController : Controller
    {
        private readonly UserManager<AppUserModel> _userUpdate;

        private readonly SignInManager<AppUserModel> _signInManager;


        public PersonalProfileController(
            UserManager<AppUserModel> userManager,
            SignInManager<AppUserModel> signInManager)
        {
            _userUpdate = userManager;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Update(string? Name, string? Email, string? PhoneNumber, string? Address)
        {
            // Retrieve the currently logged-in user
            var user = await _userUpdate.GetUserAsync(User);

            // Update or add new information based on whether fields are provided
            if (!string.IsNullOrEmpty(Name))
            {
                user.Name = Name; // Update Name if provided
            }

            if (!string.IsNullOrEmpty(Name))
            {
                user.Email = Email; // Update Email if provided
            }

            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                user.PhoneNumber = PhoneNumber; // Update PhoneNumber if provided
            }

            if (!string.IsNullOrEmpty(Address))
            {
                user.Address = Address; // Update Address if provided
            }

            // Save changes to the database
            var result = await _userUpdate.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Handle any errors during the update
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(user); // Return the view with error messages
            }

            // Redirect to the Index page or a confirmation page after successful update
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            // Kiểm tra nếu thông tin nhập bị thiếu
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin.";
                return RedirectToAction(nameof(Index)); // Ngừng xử lý và quay về trang Index
            }

            // Kiểm tra nếu mật khẩu mới và mật khẩu xác nhận không khớp
            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return RedirectToAction(nameof(Index)); // Ngừng xử lý và quay về trang Index
            }

            // Lấy thông tin người dùng hiện tại
            var user = await _userUpdate.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("Login");
            }

            // Kiểm tra mật khẩu hiện tại
            var passwordCheck = await _userUpdate.CheckPasswordAsync(user, currentPassword);
            if (!passwordCheck)
            {
                TempData["ErrorMessage"] = "Mật khẩu hiện tại không đúng.";
                return RedirectToAction(nameof(Index));
            }

            // Thực hiện đổi mật khẩu
            var result = await _userUpdate.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Xử lý lỗi khi đổi mật khẩu thất bại
            foreach (var error in result.Errors)
            {
                TempData["ErrorMessage"] += error.Description + "<br>";
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
