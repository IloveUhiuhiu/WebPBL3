using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TextTemplating;
using System.Drawing;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class CarController : Controller
    {
        private ApplicationDbContext _db;
        public CarController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult CarListTable()
        {
            List<CarDto> _cars = _db.Cars
                .Include(c => c.Make)
                .Select(c => new CarDto
                {
                    CarID = c.CarID,
                    CarName = c.CarName,
                    Price = c.Price,
                    Seat = c.Seat,
                    Origin = c.Origin,
                    Dimension = c.Dimension,
                    Capacity = c.Capacity,
                    Topspeed = c.Topspeed,
                    Color = c.Color,
                    Photo = c.Photo,
                    Year = c.Year,
                    Engine = c.Engine,
                    Quantity = c.Quantity,
                    Description = c.Description,
                    FuelConsumption = c.FuelConsumption,
                    MakeName = c.Make.MakeName
                })
                .ToList();
            return View(_cars);
        }
        // [GET]
        public IActionResult Create()
        {
            return View();
        }
        // [POST]
        [HttpPost]
        public IActionResult Create(CarDto car)
        {
            return View(car);
        }
        public IActionResult Update()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
	}
}
