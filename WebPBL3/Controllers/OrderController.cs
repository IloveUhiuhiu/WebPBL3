using Microsoft.AspNetCore.Mvc;

namespace WebPBL3.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
