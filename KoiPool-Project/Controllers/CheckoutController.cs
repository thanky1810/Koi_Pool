using KoiPool_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiPool_Project.Models.ViewModels;
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

        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var orderCode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = orderCode;
                orderItem.UserName = userEmail;
                orderItem.status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _context.Add(orderItem);
                _context.SaveChanges();
                TempData["success"] = "Đơn hàng được tạo";
                return RedirectToAction("Index");

            }
            return View();
        }
    }
}

