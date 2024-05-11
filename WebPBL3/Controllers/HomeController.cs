using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;   
        }

        public IActionResult Index()
        {
            ViewBag.HideHeader = false;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult About()
        {
            return View(); 
        }
        public IActionResult Contact()
        {   
            List<Province> provinces = _db.Provinces.ToList();
            return View(provinces);
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
