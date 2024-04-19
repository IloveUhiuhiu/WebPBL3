using Microsoft.AspNetCore.Mvc;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DanhSachTinTuc()
        {
            return View(model: StaticNews.AllNews);
        }
        public IActionResult ThemTinTuc()
        {
            return View();
        }
    }
}
