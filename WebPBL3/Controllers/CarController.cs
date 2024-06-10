using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using WebPBL3.Models;
using WebPBL3.DTO;
using Microsoft.AspNetCore.Authorization;
using WebPBL3.Services;
using Microsoft.Identity.Client;
namespace WebPBL3.Controllers
{
    public class CarController : Controller
    {
        private ICarService _carService;
        private IPhotoService _photoService;
        // Số lượng item mỗi trang
        private int limits = 10;
        public CarController(ICarService carService, IPhotoService photoService)
        {
            _carService = carService;
            _photoService = photoService;
        }

        /*public async Task<IActionResult> Index(string searchTerm = "")
        {
			ViewBag.HideHeader = false;
			ViewBag.SearchTerm = searchTerm;

            var makes = await _db.Makes.ToListAsync();
            var origins = await _db.Cars.Select(c => c.Origin).Distinct().ToListAsync();
            var colors = await _db.Cars.Select(c => c.Color).Distinct().ToListAsync();
            var fuels = await _db.Cars.Select(c => c.FuelConsumption).Distinct().ToListAsync();
            var seats = await _db.Cars.Select(c => c.Seat).Distinct().ToListAsync();

            // Đưa danh sách vào ViewBag
            ViewBag.Makes = new SelectList(makes, "MakeID", "MakeName");
            ViewBag.Origins = new SelectList(origins);
            ViewBag.Colors = new SelectList(colors);
            ViewBag.Fuels = new SelectList(fuels);
            ViewBag.Seats = new SelectList(seats);

            return View();
        }

        public ActionResult Cars(string txtSearch = "",string makeName = "", string origin = "", string color = "", string seat = "",int page = 1, int perPage = 9, string sortBy = "")
        {
			var item = _db.Cars
		    .Include(c => c.Make)
		    .Where(c => !c.Flag) // Lọc những xe không bị gắn cờ
		    .ToList();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                item = item.Where(car => car.CarName.ToLower().Contains(txtSearch.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(makeName))
            {
                item = item.Where(c => c.Make.MakeName == makeName).ToList();
            }
			if (!string.IsNullOrEmpty(origin))
			{
				item = item.Where(c => c.Origin == origin).ToList();
			}
			if (!string.IsNullOrEmpty(color))
			{
				item = item.Where(c => c.Color == color).ToList();
			}
			if (!string.IsNullOrEmpty(seat) && int.TryParse(seat, out int seatNumber))
			{
				item = item.Where(c => c.Seat == seatNumber).ToList();
			}
            switch(sortBy)
            {
                case "Price":
                    item = item.OrderBy(p => p.Price).ToList();
                    break;
                case "bestSelling":
                    var orders = _db.Orders
                        .Include(o => o.DetailOrders)
                        .ThenInclude(deo => deo.Car)
                        .Where(o => o.Status == "Đã thanh toán")
                        .ToList();
                    Dictionary<string, int> quantity = new Dictionary<string, int>();
                    foreach (var car in item)
                    {
                        quantity[car.CarID] = 0;
                    }
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
                    item = item.OrderByDescending(c => quantity[c.CarID]).ToList();
                    break;
            }
            int totalCount = item.Count();
            var cars = item.Skip((page-1) * perPage)
                .Take(perPage)
                .Select(i => new List<string>
            {
                i.CarID,
                i.Photo,
                i.Price.ToString(),
                i.CarName
            }) ;
            int totalPages = (int)Math.Ceiling((double)totalCount / perPage);
            return Json(new { Data = cars, TotalPages = totalPages });
		}

		public IActionResult Detail(string id)
        {
			ViewBag.HideHeader = false;
			if (String.IsNullOrEmpty(id))
			{
				return NotFound();
			}
			Car? c = _db.Cars.Find(id);

			if (c == null)
			{
				return NotFound();
			}
			var makeName = _db.Makes.Where(m => m.MakeID == c.MakeID).FirstOrDefault().MakeName;
			CarDTO CarDTOFromDb = new CarDTO
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
				MakeName = makeName,

			};
            var relatedCars = _db.Cars
                        .Where(car => car.MakeID == c.MakeID && car.CarID != c.CarID && !car.Flag)
                        .ToList();
            ViewBag.RelatedCars = relatedCars;
            return View(CarDTOFromDb);
        }*/

        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CarListTable(int makeid = 0,string searchtxt = "", int page = 1)
        {

            // Kiểm tra và lấy dữ liệu "makes" nếu chưa có trong TempData
            // TemData lưu trữ dữ liệu trong Session, khi Session hết hạn hoặc bị xóa thì makes bay
            if (!TempData.ContainsKey("makes"))
            {
                IEnumerable<Make> makes = await _carService.GetAllMakes();
                TempData["makes"] = JsonConvert.SerializeObject(makes);
                TempData.Keep("makes");
            }

            int total = await _carService.CountCars(makeid, searchtxt, page);
            // tổng số trang
            var totalPage = (total + limits - 1) / limits;
            // sử dụng khi previous là 1
            if (page < 1) page = 1;
            // sử dụng khi next là totalPage 
            if (page > totalPage) page = totalPage;
            IEnumerable<CarDTO> cars = await _carService.GetAllCars(makeid, searchtxt, page);
            // tổng số sản phẩm
            

            ViewBag.totalRecord = total;
            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = page;
            ViewBag.makeid = makeid;
            ViewBag.searchtxt = searchtxt;
            
             
            return View(cars);
        }
        // [GET]
        [Authorize(Roles = "Admin, Staff")]
        public IActionResult Create()
        {
            return View();
        }
        // [POST]
        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Create(CarDTO cardto, IFormFile uploadimage)
        {
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {


                cardto.CarID = await _carService.GenerateID();

                if (uploadimage != null && uploadimage.Length > 0)
                {
                    cardto.Photo = await _photoService.AddPhoto("car", uploadimage);
                }
                try
                {
                    
                    await _carService.AddCar(cardto);
                        
                }
                catch (DbUpdateException ex)
                {
                    // 404
                    return BadRequest("Error add car: " + ex.Message);

                }
                return RedirectToAction("CarListTable");

            }
            return View(cardto);

        }
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id is null");
            }
            Car? car = await _carService.GetCarById(id);
            
            if (car == null)
            {
                return NotFound("Car is not found");
            }

            CarDTO CarDTOFromDb = _carService.ConvertToCarDTO(car);
            
            return View(CarDTOFromDb);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Edit(CarDTO cardto,IFormFile? uploadimage)
        {
            
            if (ModelState.IsValid)
            {
                ;
                if (string.IsNullOrEmpty(cardto.CarID))
                {
                    return NotFound("Car is not found");
                }
                if (uploadimage != null && uploadimage.Length > 0)
                {
                    cardto.Photo = await _photoService.EditPhoto("car",uploadimage,cardto.Photo);
                }
               
                try
                {
                    await _carService.EditCar(cardto);
                        
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // 404
                    return BadRequest("Error edit car: " + ex.Message);

                }
                return RedirectToAction("CarListTable");
            }
            return View(cardto);

        }
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Details(string? id)
        {
            
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id is null");
            }
            Car? car = await _carService.GetCarById(id);

            if (car == null)
            {
                return NotFound("Car is not found");
            }
            CarDTO cardtoFromDb = _carService.ConvertToCarDTO(car);

            return View(cardtoFromDb);
        }
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Delete(string? id)
        {
            
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Id is null");
            }
            Car? car = await _carService.GetCarById(id);

            if (car == null)
            {
                return NotFound("Car is not found");
            }
           
            try
            {
               await _carService.DeleteCar(car);
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                
                return BadRequest("Error delete car: " + ex.Message);
            }

            return RedirectToAction("CarListTable");
            
        }


	}
}
