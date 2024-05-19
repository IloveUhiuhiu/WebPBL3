using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
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
            _db = db;
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
                var user = _db.Accounts.Include(u => u.Role).FirstOrDefault(u => u.Email == model.Email);
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
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        
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
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.RetypePassword)
                {
                    TempData["Error"] = "Mật khẩu nhập lại không khớp";
                    return View();
                }
                var currentAccount = _db.Accounts.FirstOrDefault(u => u.Email == model.Email);
                if (currentAccount != null)
                {
                    TempData["Error"] = "Email đã tồn tại";
                    return View();
                }
                Role role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
                var newAccount = new Account
                {
                    AccountID = Guid.NewGuid().ToString().Substring(0, 10),
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Status = true,
                    RoleID = role.RoleID,
                };
                _db.Accounts.Add(newAccount);
                var user = new User
                {
                    UserID = Guid.NewGuid().ToString().Substring(0, 10),
                    AccountID = newAccount.AccountID
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Denied()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var existAccount = _db.Accounts.FirstOrDefault(u => u.Email == email);
            if (existAccount!=null)
            {
                string otp = GenerateOTP();
                HttpContext.Session.SetString("OTP",otp);
                string message = "Mã OTP: " + otp;
                existAccount.Password= BCrypt.Net.BCrypt.HashPassword(otp);
                _db.Accounts.Update(existAccount);
                await _db.SaveChangesAsync();
                await SendMailGoogleSmtp(email, "Thiết lập mật khẩu mới", message);
                return RedirectToAction("setupNewPassword", new { email = email });
            }
            else
            {
                TempData["Error"] = "Email không tồn tại!";
                return View();
            }
        }
        public static async Task SendMailGoogleSmtp( string _to, string _subject,
                                                            string _body)
        {
            string _mail = "thanhpnwork22@gmail.com";
            string _pw = "imid wxgq ttvd ndaz";
            MailMessage message = new MailMessage(
                from: _mail,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            var client = new SmtpClient("smtp.gmail.com",587)
            {
                Credentials = new NetworkCredential(_mail, _pw),
                EnableSsl = true
            };  
            await client.SendMailAsync(message);
        }
        private string GenerateOTP()
        {
            Random random = new Random();
            string result = "";
            for (int i = 0; i < 6; i++)
            {
                result += random.Next(0, 10);
            }
            return result;
        }
        public IActionResult setupNewPassword(string  email)
        {
            ViewBag.Email = email;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> setupNewPassword(string otp, string password,
            string retypepassword,string email)
        {
            if (password != retypepassword)
            {
                TempData["Error"] = "Mật khẩu nhập lại không khớp";
                return View();
            }
            var sessionValue = HttpContext.Session.GetString("OTP");
            if (sessionValue != otp &&otp != null)
            {
                TempData["Error"] = "Mã OTP không hợp lệ";
                return View();
            }
            if (email == null)
            {
                TempData["Error"] = "Xảy ra lỗi khi truyền dữ liệu. Vui lòng thao tác lại";
                return View();
            }
            var existAccount = _db.Accounts.FirstOrDefault(u => u.Email == email);
            if (existAccount != null)
            {
                existAccount.Password = BCrypt.Net.BCrypt.HashPassword(password);
                _db.Accounts.Update(existAccount);
                await _db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}

