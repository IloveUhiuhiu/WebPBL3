using Microsoft.AspNetCore.Mvc;

namespace WebPBL3.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
