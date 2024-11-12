using Microsoft.AspNetCore.Mvc;

namespace KoiPool_Project.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
