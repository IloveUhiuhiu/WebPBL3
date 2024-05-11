using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Numerics;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _db;
        private IWebHostEnvironment _environment;
        private int limits = 10;
        public UserController (ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET
        public IActionResult UserListTable(string searchtxt ="", int page = 1)
        {
            if (!TempData.ContainsKey("wards"))
            {
                List<Ward> wards = _db.Wards.ToList();
                List<District> districts = _db.Districts.ToList();
                List<Province> provinces = _db.Provinces.ToList();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                TempData["wards"] = wards;
                TempData["wards"] = JsonConvert.SerializeObject(wards, settings);
                TempData.Keep("wards");
                TempData["districts"] = districts;
                TempData["districts"] = JsonConvert.SerializeObject(districts, settings);
                TempData.Keep("districts");
                TempData["provinces"] = provinces;
                TempData["provinces"] = JsonConvert.SerializeObject(provinces, settings);
                TempData.Keep("provinces");
             

            }

            List<UserDto> users = _db.Users.Include(a => a.Account).Include(w => w.Ward).Where(u => (searchtxt.IsNullOrEmpty() || u.FullName.Contains(searchtxt))).Select(u => new UserDto
            { 
				AccountID = u.AccountID,
		        Email = u.Account.Email,
		        Password = u.Account.Password,
		        Status = u.Account.Status,
		    
		        RoleID = 3,
		        UserID = u.UserID,
		        FullName = u.FullName,
		        PhoneNumber =u.PhoneNumber,
		        IdentityCard =u.IdentityCard,

		        Gender =u.Gender,
		        BirthDate =u.BirthDate,
		        Address =u.Address,
		        Photo = u.Photo,
		        WardID =u.WardID,
		        WardName =u.Ward.WardName,
		        DistrictName = u.Ward.District.DistrictName,
		        ProvinceName = u.Ward.District.Province.ProvinceName,

			}).ToList();
            var total = users.Count;
            var totalPage = (total + limits - 1) / limits;
            if (page < 1) page = 1;
            if (page > totalPage) page = totalPage;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = page;
            users = users.Skip((page - 1) * limits).Take(limits).ToList();
            int cnt = 0;
            foreach (var user in users)
            {
                user.STT = ++cnt;
            }
            return View(users);
        }
        // GET
        public IActionResult Create()
        {
            
            return View();
        }
        // POST
        [HttpPost]
        public async Task<IActionResult> Create(UserDto user, IFormFile? uploadimage)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error: {error.ErrorMessage}");
            }
            if (ModelState.IsValid)

            {   
                // Account
                var accid = 1;
                var lastAcc = _db.Accounts.OrderByDescending(a => a.AccountID).FirstOrDefault();
                if (lastAcc != null)
                {
                    accid = Convert.ToInt32(lastAcc.AccountID)+1;
                }
                Console.WriteLine(accid + user.Email);
                var accidTxt = accid.ToString().PadLeft(8,'0');
                user.AccountID = accidTxt;
                var accWithEmail = _db.Accounts.FirstOrDefault(u => u.Email == user.Email);
                if (accWithEmail != null)
                {
                    TempData["Error"] = "Email đã tồn tại";
                    return View(user);
                }
                // Add Account
                _db.Accounts.Add(new Account
                {
                    AccountID = user.AccountID,
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Status = false,
                    RoleID = 3,
                });
                await _db.SaveChangesAsync();
                // User
                var userid = 1;
                var lastUser = _db.Users.OrderByDescending(u => u.UserID).FirstOrDefault();
                if (lastUser != null)
                {
                    userid = Convert.ToInt32(lastUser.UserID.Substring(2)) + 1;
                }
                var useridTxt = "KH" + userid.ToString().PadLeft(6, '0');
                user.UserID = useridTxt;
                if (uploadimage != null)
                {
                    int index = uploadimage.FileName.IndexOf('.');
                    string _FileName = "user" + user.UserID + "." + uploadimage.FileName.Substring(index + 1);
                    user.Photo = _FileName;
                }
                
                _db.Users.Add(new User
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    IdentityCard = user.IdentityCard,
                    Gender = user.Gender,
                    Address = user.Address,
                    BirthDate = user.BirthDate,
                    Photo = user.Photo,
                    WardID = user.WardID,
                    AccountID = user.AccountID,

                });

                await _db.SaveChangesAsync();
                if (uploadimage != null && uploadimage.Length > 0 )
                {
                    string _path = Path.Combine(_environment.WebRootPath, "upload\\user", user.Photo);
                    using (var fileStream = new FileStream(_path, FileMode.Create))
                    {
                        await uploadimage.CopyToAsync(fileStream);

                    }
                }
                return RedirectToAction("UserListTable");

            }
            return View(user);
        }
        // GET
        public IActionResult Edit(string ?id)
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Edit(UserDto user)
        {
            return View();
        }
        // GET
        public IActionResult Details(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            User? user = _db.Users.Find(id);
            
            if (user == null)
            {
                return NotFound();
            }

            Account account = _db.Accounts.Where(a => a.AccountID == user.AccountID).FirstOrDefault();
            Ward ward = new Ward();
            District district = new District();
            Province province = new Province();
            ward = _db.Wards.Where(w => w.WardID == user.WardID).FirstOrDefault();
            if (ward != null)
            {
                district = _db.Districts.Where(d => d.DistrictID == ward.DistrictID).FirstOrDefault();
                if (district != null) province = _db.Provinces.Where(p => p.ProvinceID == district.ProvinceID).FirstOrDefault();
            }
            UserDto userFromDb = new UserDto
            {
                UserID = user.AccountID,
                FullName = user.FullName,
                Email = account.Email,
                PhoneNumber = user.PhoneNumber,
                IdentityCard = user.IdentityCard,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                Address = user.Address,
                ProvinceName = province.ProvinceName,
                DistrictName = district.DistrictName,
                WardName = ward.WardName,  
                Photo = user.Photo,
            };

            return View(userFromDb);
        }
        // POST
        [HttpPost]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
