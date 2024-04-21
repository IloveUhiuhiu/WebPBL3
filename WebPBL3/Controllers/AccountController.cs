using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebPBL3.Models;
using WebPBL3.ViewModel;

namespace WebPBL3.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountController(ApplicationDbContext db)
        {
            _db= db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user=_db.Accounts.Include(u => u.Role).FirstOrDefault(u=>u.Email==model.Email);
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.Role,user.Role.RoleName)
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal=new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Error"] = "Tài khoản hoặc mật khẩu không hợp lệ";
                        return View();
                    }
                }
                TempData["Error"] = "Tài khoản hoặc mật khẩu không hợp lệ";
                return View();
            }
            else
            {
                return View();
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Register(RegisterVM model)
        {
            if(ModelState.IsValid)
            {
                if(model.Password !=model.RetypePassword)
                {
                    TempData["Error"] = "Mật khẩu nhập lại không khớp";
                    return View();
                }
                var currentAccount=_db.Accounts.FirstOrDefault(u=>u.Email==model.Email);
                if(currentAccount!=null)
                {
                    TempData["Error"] = "Email đã tồn tại";
                    return View();
                }
                Role role=await _db.Roles.FirstOrDefaultAsync(r=>r.RoleName=="User");
                var newAccount = new Account
                {
                    AccountID = Guid.NewGuid().ToString().Substring(0,10),
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Status = true,
                    RoleID = role.RoleID,
                };
                _db.Accounts.Add(newAccount);
                await _db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Denied()
        {
            return View();
        }
    }
}
