using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
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
            
            List<CarDto> cars = _db.Cars.Include(c => c.Make).Select (c => new CarDto
            {
                CarID = c.CarID,
                CarName = c.CarName,
                Photo = c.Photo,

                Capacity = c.Capacity,
                FuelConsumption = c.FuelConsumption,
                Color = c.Color,

                Description = c.Description,
                Dimension = c.Dimension,
                Engine = c.Engine,

                Origin = c.Origin,
                Price  = c.Price,
                Quantity = c.Quantity,

                Seat = c.Seat,
                Topspeed = c.Topspeed,
                Year = c.Year,

                MakeID = c.MakeID,
                MakeName = c.Make.MakeName,
            }).ToList();
            return View(cars);
        }
        // [GET]
        public IActionResult Create()
        {
            List<Make> makes = _db.Makes.ToList();
            TempData["makes"] = JsonConvert.SerializeObject(makes);
            return View();
        }
        // [POST]
        [HttpPost]
        public IActionResult Create(CarDto c)
        {
            foreach (var state in ViewData.ModelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
              
            {
                var carid = 1;
                var lastCar = _db.Cars.OrderByDescending(car => car.CarID).FirstOrDefault();
                if (lastCar != null)
                {
                    carid = Convert.ToInt32(lastCar.CarID) + 1;
                }
                var caridTxt = carid.ToString().PadLeft(8, '0');
                c.CarID = caridTxt;
                
                _db.Cars.Add( new Car
                {
                    CarID = c.CarID,
                    CarName = c.CarName,
                    Photo = c.Photo,

                    Capacity = c.Capacity,
                    FuelConsumption = c.FuelConsumption,
                    Color = c.Color,

                    Description = c.Description,
                    Dimension = c.Dimension,
                    Engine = c.Engine,

                    Origin = c.Origin,
                    Price = c.Price,
                    Quantity = c.Quantity,

                    Seat = c.Seat,
                    Topspeed = c.Topspeed,
                    Year = c.Year,

                    MakeID = c.MakeID,

                });
                    
                _db.SaveChanges();
                
                return RedirectToAction("CarListTable");
            }
            return View();
            
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
