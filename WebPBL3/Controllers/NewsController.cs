using Microsoft.AspNetCore.Mvc;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext _db;
        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<News> list = _db.NewS.ToList();
            return View(list);
        }
        public IActionResult ListNews()
        {
            List<News> DanhSachTinTuc = _db.NewS.ToList();
            return View(DanhSachTinTuc);
        }
    }
}
