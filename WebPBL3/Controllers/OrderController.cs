using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPBL3.DTO;
using WebPBL3.Models;
using System.Text.Json;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebPBL3.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext db)
        {
            _context = db;
        }
        public IActionResult Index(int page = 1)
        {
            string orderJson = TempData["orders"] as string;
            if(TempData["status"] != null)
            {
                ViewBag.status = TempData["status"] as string;
                
            }
            if (TempData["idUser"] != null)
            {
                ViewBag.idUser = TempData["idUser"] as string;
            }
            List<Order> orders;
            if (orderJson != null)
            {
                orders = JsonConvert.DeserializeObject<List<Order>>(orderJson);
            }
            else
            {
                orders = _context.Orders.ToList();
            }
            double total = orders.Count;
            var totalPage = (int)Math.Ceiling(total / 10);
            if (page < 1)
                page = 1;
            if (page > totalPage) 
                page = totalPage;
            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = page;
            orders = orders.Skip((page - 1) * 10).Take(10).ToList();
            return View(orders);
        }
        public IActionResult Creat() 
        {
            OrderDTO orderDTO;
            string orderDTOJson = TempData["orderDTO"] as string;
            if (orderDTOJson == null)
            {
                orderDTO = new OrderDTO();
            }
            else
            {
                orderDTO = JsonConvert.DeserializeObject<OrderDTO>(orderDTOJson);
            }
            return View(orderDTO);
        }
        [HttpPost]
        public IActionResult ExtractEmail(string existEmail)
        {
            User? u = _context.Users
                .Include(a => a.Account)
                .FirstOrDefault(u => u.Account.Email == existEmail);
            if (u != null)
            {
                OrderDTO orderDTO = new OrderDTO
                {
                    FullName = u.FullName,
                    IdentityCard = u.IdentityCard,
                    PhoneNumber = u.PhoneNumber,
                    Email = existEmail,
                    Address = u.Address,
                };
                string orderDTOJson = JsonConvert.SerializeObject(orderDTO);
                TempData["orderDTO"] = orderDTOJson;
            }
            return RedirectToAction("Creat");
        }
        [HttpPost]
        public IActionResult AddItem(OrderDTO orderDTO,string CarID, int quantity)
        {
            var car = _context.Cars.Find(CarID);
            if (car == null)
            {
                return NotFound();
            }
            Items item = new Items
            {
                CarID = car.CarID,
                quantity = quantity,
                CarName = car.CarName,
                price = car.Price
            };
            orderDTO.items.Add(item);
            string orderDTOJson = JsonConvert.SerializeObject(orderDTO);
            TempData["orderDTO"] = orderDTOJson;
            return RedirectToAction("Creat");
        }
        [HttpPost]
        public IActionResult DeleteItem(OrderDTO orderDTO, string delItemId)
        {
            orderDTO.items.RemoveAll(item => item.CarID == delItemId);
            string orderDTOJson = JsonConvert.SerializeObject(orderDTO);
            TempData["orderDTO"] = orderDTOJson;
            return RedirectToAction("Creat");
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderDTO orderDTO)
        {
            User? u = _context.Users
                .Include(a => a.Account)
                .FirstOrDefault(u => u.Account.Email == orderDTO.Email);
            Staff? staff=_context.Staffs
                .Include(u=>u.User.Account)
                .FirstOrDefault(u=>u.User.Account.Email== HttpContext.User.FindFirstValue(ClaimTypes.Name));
            if(u == null)
            {
                Account a = new Account
                {
                    Email = orderDTO.Email,
                    AccountID = Guid.NewGuid().ToString().Substring(0, 10),
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Status = true,
                    RoleID = 1,
                };
                _context.Accounts.Add(a);
                u = new User
                {
                    UserID = Guid.NewGuid().ToString().Substring(0, 10),
                    FullName = orderDTO.FullName,
                    Address = orderDTO.Address,
                    IdentityCard = orderDTO.IdentityCard,
                    PhoneNumber = orderDTO.PhoneNumber,
                    AccountID=a.AccountID,  
                };
                _context.Users.Add(u);
            }
            var order_id = 1;
            var lastOrder = _context.Orders.OrderByDescending(o => o.OrderID).FirstOrDefault();
            if (lastOrder != null)
            {
                order_id = Convert.ToInt32(lastOrder.OrderID.Substring(2)) + 1;
            }
            var orderID = "DH" + order_id.ToString().PadLeft(6, '0');
            Order order = new Order
            {
                OrderID = orderID,
                Date = orderDTO.Date,
                Totalprice = orderDTO.Totalprice,
                Status = orderDTO.Status,
                Flag = true,
                UserID = u.UserID,
                StaffID = staff.StaffID
            };
            _context.Orders.Add(order);
            foreach (var item in orderDTO.items)
            {
                DetailOrder detailOrder = new DetailOrder
                {
                    DetailOrderID = Guid.NewGuid().ToString().Substring(0, 10),
                    Quantity = item.quantity,
                    Price = item.price,
                    OrderID = order.OrderID,
                    CarID = item.CarID
                };
                _context.DetailOrders.Add(detailOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(string id)
        {
            Order? order = _context.Orders
                .Include(u => u.User.Account)
                .Include(st => st.Staff)
                .FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            var details = _context.DetailOrders
                     .Include(c => c.Car)
                     .Where(o => o.OrderID == id)
                     .ToList();
            ViewBag.Order = order;
            ViewBag.ListDetail = details;
            return View();
        }
        public async Task<IActionResult> DeleteOrder(string id)
        {
            Order o=_context.Orders.FirstOrDefault(o=>o.OrderID == id);
            if (o != null)
            {
                _context.Orders.Remove(o);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditOrder(string id)
        {
            Order o = _context.Orders.FirstOrDefault(o => o.OrderID == id);
            if (o != null) 
            {
                o.Status = "Đã thanh toán";
                _context.Update(o);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public IActionResult FilterOrder(string status,string idUser)
        {
            List<Order> orders=_context.Orders.Where(o=>(status.IsNullOrEmpty()||o.Status.Contains(status))&&(idUser.IsNullOrEmpty()||o.UserID.Contains(idUser))).ToList();
            string orderJson = JsonConvert.SerializeObject(orders);
            TempData["orders"] = orderJson;
            TempData["status"]=status;
            TempData["idUser"] = idUser;
            return RedirectToAction("Index");
        }
    }
}
