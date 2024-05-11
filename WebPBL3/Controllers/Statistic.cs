using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class Statistic : Controller
    {   
        ApplicationDbContext _db;
       
        public Statistic (ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult>  StatisticStaff()
        {   
            List<Staff> staffs = await _db.Staffs.ToListAsync();
            List<Car> cars = await _db.Cars.ToListAsync();
            List<User> users = await _db.Users.ToListAsync();
            List<Order> orders = await _db.Orders.Include(deo => deo.DetailOrders).ToListAsync();
            ViewBag.staffTotal = staffs.Count;
            ViewBag.carTotal = cars.Count;
            ViewBag.userTotal = users.Count;
            int revenueTotal = 0;
            
            foreach (Order order in orders)
            {   
                if (order.Status == "Đã thanh toán")
                {    
                    revenueTotal += order.Totalprice;
                    
                }   
            }
            ViewBag.revenueTotal = revenueTotal;

            return View();
        }
        public async Task<IActionResult> StatisticCar(int makeId = 0) {
            List<Staff> staffs = await _db.Staffs.ToListAsync();
            List<Car> cars = await _db.Cars.Take(10).ToListAsync();
            List<User> users = await _db.Users.ToListAsync();
            List<Order> orders = await _db.Orders.Include(deo => deo.DetailOrders).ToListAsync();
            List<Make> makes = await _db.Makes.ToListAsync();
            ViewBag.staffTotal = staffs.Count;
            ViewBag.carTotal = cars.Count;
            ViewBag.userTotal = users.Count;
            ViewBag.makes = makes;
            int revenueTotal = 0;

            foreach (Order order in orders)
            {
                if (order.Status == "Đã thanh toán")
                {
                    revenueTotal += order.Totalprice;
                }
            }
            ViewBag.revenueTotal = revenueTotal;

            // Lay danh sach ten hang
            List<string> nameMake = new List<string>();
            foreach (var make in makes) nameMake.Add(make.MakeName);
            ViewBag.nameMake = nameMake.ToArray();
            // Lay danh sach ten xe
            List<string> nameCar = new List<string>();
            foreach (var car in cars) nameCar.Add(car.CarName);
            ViewBag.nameCar = nameCar.ToArray();
            // Lay danh thu cua xe
            List<int> revenueCar = new List<int>();
            foreach (var car in cars)
            {
                int Total = 0;
                foreach (var order in orders)
                {
                    foreach (var detailOrder in order.DetailOrders)
                    {
                        if (detailOrder.CarID == car.CarID) Total += detailOrder.Price;
                    }
                }
            }
            ViewBag.revenueCar = revenueCar.ToArray();
            // Lay doanh thu cua hang
            List<int> revenueMake = new List<int>();
           
            foreach (var make in makes)
            {
                
                int Total = 0;
                int length = revenueCar.Count;
                for (int i=0; i<length; i++)
                {
                    if (cars[i].MakeID == make.MakeID)
                    {
                        Total += revenueCar[i];
                    }
                }
            }
            ViewBag.revenueMake = revenueMake.ToArray();
            return View();

        }
    }
}
