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


        public PersonalProfileController(
            UserManager<AppUserModel> userManager)
        {
            _userUpdate = userManager;
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

            if (!string.IsNullOrEmpty(Email))
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

    }
}
