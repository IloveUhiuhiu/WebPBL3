using Microsoft.AspNetCore.Mvc;

namespace WebPBL3.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
