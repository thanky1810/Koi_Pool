using Microsoft.AspNetCore.Mvc;

namespace KoiPool_Project.Controllers
{
    public class PersonalProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
