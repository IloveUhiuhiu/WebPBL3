using Microsoft.AspNetCore.Mvc;

namespace WebPBL3.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
	}
}
