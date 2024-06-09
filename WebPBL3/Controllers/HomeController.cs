using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebPBL3.DTO;
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

        public async Task<IActionResult> Index()
        {
            ViewBag.HideHeader = false;
            List<Order> orders = await _db.Orders
                .Include(o => o.DetailOrders)
                .ThenInclude(deo => deo.Car)
                .ThenInclude(c => c.Make)
                .Where(o => o.Status == "Đã thanh toán")
                .ToListAsync();
            Dictionary<string, int> quantity = new Dictionary<string, int>();
            foreach (var order in orders)
            {
                foreach (var detailOrder in order.DetailOrders)
                {
                    if (!quantity.ContainsKey(detailOrder.Car.CarID))
                    {
                        quantity[detailOrder.Car.CarID] = 0;
                    }
                    quantity[detailOrder.Car.CarID] += detailOrder.Quantity;  
                }
            }
            var sortedDict = quantity.OrderBy(q => q.Value).Take(4).ToList();
            List<CarDTO> cars = new List<CarDTO>();
            foreach (var item in sortedDict) {
                Car? _car = await _db.Cars.Include(c => c.Make).FirstOrDefaultAsync(c => c.CarID == item.Key);


                cars.Add(new CarDTO
                {
                    CarID = item.Key,
                    CarName = _car.CarName,
                    MakeName = _car.Make.MakeName,
                    Price = _car.Price,
                    Photo = _car.Photo
                });
                 
                
            }
            

            List<Feedback> _feedBacks = await _db.Feedbacks.Include(fb => fb.User).Take(5).ToListAsync();
            List<FeedBackHomeDTO> feedBacks = new List<FeedBackHomeDTO>();


            foreach (var item in _feedBacks)
            {   
                User? user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == item.UserID);

                feedBacks.Add(new FeedBackHomeDTO
                {
                    FullName = item.FullName,
                    Title = item.Title,
                    Content = item.Content,
                    Photo = (user != null) ? user.Photo : string.Empty
                }) ;
            }

            
            


            List<News> _news = await _db.NewS.Include(n => n.Staff).ThenInclude( s => s.User).OrderByDescending(s => s.CreateAt).Take(3).ToListAsync();
            List<NewsDTO> news = new List<NewsDTO>();
            foreach (var item in _news)
            {
                news.Add(new NewsDTO
                {   
                    NewsID = item.NewsID,
                    Photo = item.Photo,
                    FullName = item.Staff.User.FullName,
                    Title = item.Title,
                    CreateAt = item.CreateAt,
                });
            }
            
            ViewBag.news = news;
            ViewBag.cars = cars; 
            ViewBag.feedBacks = feedBacks;
            return View();

        }

        public IActionResult About()
        {
            ViewBag.HideHeader = false;
            return View(); 
        }
        public IActionResult Contact()
        {
            ViewBag.HideHeader = false;
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
