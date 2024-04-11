using Microsoft.AspNetCore.Mvc;

namespace WebPBL3.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
