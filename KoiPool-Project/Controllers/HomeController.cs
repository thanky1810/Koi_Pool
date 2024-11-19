
using KoiPool_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.AccessControl;

namespace KoiPool_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult GioiThieu()
        {
            return View();
        }
        public IActionResult DuAn()
        {
            return View();
        }
        public IActionResult DichVu()
        {
            return View();
        }
        public IActionResult BaoGia()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult LienHe()
        {
            return View();
        }
        public IActionResult Baroque()
        {
            return View();
        }
        public IActionResult Nganhthietke()
        {
            return View();
        }
        public IActionResult Nganhkientruc()
        {
            return View();
        }
        public IActionResult Nhachoi()
        {
            return View();
        }
        public IActionResult Cafe()
        {
            return View();
        }
        public IActionResult Vuon()
        {
            return View();
        }
        public IActionResult Thongtin()
        {
            return View();
        }
        public IActionResult Koicafe()
        {
            return View();
        }
        public IActionResult Cayxanh()
        {
            return View();
        }
        public async Task<IActionResult> QuanLi()
        {

            return View(await _context.UserOrders.ToListAsync());
        }
        public IActionResult LichSu()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
