using KoiPool_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoiPool_Project.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _context;
        public CheckoutController(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<IActionResult> Checkout(string? FullName, string? PhoneNumber, string? Address, double? GardenArea, string? ServiceType, string? RequestContent)
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var orderCode = Guid.NewGuid().ToString();
            var orderItem = new UserOrderModel();
            orderItem.OrderCode = orderCode;
            orderItem.Email = userEmail;
            orderItem.Status = 1;
            orderItem.FullName = FullName;
            orderItem.CreatedTime = DateTime.Now;
            orderItem.PhoneNumber = PhoneNumber;
            orderItem.Address = Address;
            orderItem.GardenArea = (double)GardenArea;
            orderItem.ServiceType = ServiceType;
            orderItem.RequestContent = RequestContent;
            _context.Add(orderItem);
            _context.SaveChanges();
            TempData["success"] = "Đơn hàng được tạo";

            return RedirectToAction("LienHe", "Home");
        }
    }
}

