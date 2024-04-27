﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
using System;
using System.Drawing;
using WebPBL3.Models;


namespace WebPBL3.Controllers
{
    public class CarController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _environment;
        public CarController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
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
            if (!TempData.ContainsKey("makes"))
            {
                List<Make> makes = _db.Makes.ToList();
                TempData["makes"] = JsonConvert.SerializeObject(makes);
                TempData.Keep("makes");
            }
            List<CarDto> cars = _db.Cars.Include(c => c.Make).Where(c => c.Flag == false).Select (c => new CarDto
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
            int cnt = 0;
            foreach (var car in cars)
            {
                car.STT = ++cnt;
            }    
            return View(cars);
        }
        // [GET]
        public IActionResult Create()
        {
           
            
            
            return View();
        }
        // [POST]
        [HttpPost]
        public async Task<IActionResult> Create(CarDto c, IFormFile uploadimage)
        
        {
            

            if (ModelState.IsValid)
              
            {
                var carid = 1;
                var lastCar = _db.Cars.OrderByDescending(car => car.CarID).FirstOrDefault();
                if (lastCar != null)
                {
                    carid = Convert.ToInt32(lastCar.CarID.Substring(2)) + 1;
                }
                var caridTxt = "OT" + carid.ToString().PadLeft(6, '0');
                c.CarID = caridTxt;
                int index = uploadimage.FileName.IndexOf('.');
                string _FileName = "car" + c.CarID + "." + uploadimage.FileName.Substring(index + 1);
                c.Photo = _FileName;
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
                    
                await _db.SaveChangesAsync();
                if (uploadimage != null && uploadimage.Length > 0)
                {
                    string _path = Path.Combine(_environment.WebRootPath, "upload\\car", _FileName);
                    using (var fileStream = new FileStream(_path, FileMode.Create))
                    {
                        await uploadimage.CopyToAsync(fileStream);

					}
                }    
                return RedirectToAction("CarListTable");
                
            }
            return View(c);
            
        }
        public IActionResult Edit(string? id)
        {
            
            
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Car? c = _db.Cars.Find(id);
            
            if (c == null)
            {
                return NotFound();
            }
            
            CarDto carDtoFromDb = new CarDto
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
               
            };
            return View(carDtoFromDb);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(CarDto c,IFormFile? uploadimage)
        {
            
            if (ModelState.IsValid)
            {   
                Car car = _db.Cars.Where(ca => ca.CarID == c.CarID).FirstOrDefault();
                if (uploadimage != null && uploadimage.Length > 0)
                {
                    int index = uploadimage.FileName.IndexOf('.');
                    
                    string _FileName = "car" + c.CarID + "." + uploadimage.FileName.Substring(index+1);
                    c.Photo = _FileName;
                    string _path = Path.Combine(_environment.WebRootPath, "upload\\car", _FileName);
                    Console.WriteLine(_path);
                    using (var fileStream = new FileStream(_path, FileMode.Create))
                    {
                        await uploadimage.CopyToAsync(fileStream);

					}
                }
                
                car.CarName = c.CarName;
                car.Photo = c.Photo;

                car.Capacity = c.Capacity;
                car.FuelConsumption = c.FuelConsumption;
                car.Color = c.Color;

                car.Description = c.Description;
                car.Dimension = c.Dimension;
                car.Engine = c.Engine;

                car.Origin = c.Origin;
                car.Price = c.Price;
                car.Quantity = c.Quantity;

                car.Seat = c.Seat;
                car.Topspeed = c.Topspeed;
                car.Year = c.Year;

                car.MakeID = c.MakeID;
                
               
                try
                {
                    _db.Cars.Update(car);

                    await _db.SaveChangesAsync();
                        
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    NotFound();

                }


                return RedirectToAction("CarListTable");
            }
            return View(c);

        }


        public IActionResult Details(string? id)
        {
            
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
            CarDto carDtoFromDb = new CarDto
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
            return View(carDtoFromDb);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Car? car = _db.Cars.FirstOrDefault(c => c.CarID == id);

            if (car == null)
            {
                return NotFound();
            }
            
            car.Flag = true;
            try
            {
                _db.Cars.Update(car);

                await _db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                NotFound();

            }
            return RedirectToAction("CarListTable");
            
        }
	}
}
