using Microsoft.AspNetCore.Mvc;

namespace WebPBL3.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
