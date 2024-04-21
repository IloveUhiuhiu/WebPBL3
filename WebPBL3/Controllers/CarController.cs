using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TextTemplating;
using System.Drawing;
using WebPBL3.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            
            List<Car> cars = _db.Cars.Include(c => c.Make).ToList();
            return View(cars);
        }
        // [GET]
        public IActionResult Create()
        {
            List<Make> makes = _db.Makes.ToList();
            ViewData["makes"] = makes;
            return View();
        }
        // [POST]
        [HttpPost]
        public IActionResult Create(Car c)
        {
            
            if (ModelState.IsValid)
            {
                var carid = Convert.ToInt32(_db.Cars
                    .OrderByDescending(car => car.CarID)
                    .FirstOrDefault().CarID) + 1;
                var caridTxt = carid.ToString().PadLeft(8, '0');
                c.CarID = caridTxt;

                _db.Cars.Add(c);
                    
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
