//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebPBL3.DTO.Staff;
//using WebPBL3.Models;

//namespace WebPBL3.Controllers
//{
//    public class StaffController : Controller
//    {
//        private ApplicationDbContext _db;

//        public StaffController(ApplicationDbContext db)
//        {
//            _db = db;
//        }
//        public IActionResult listStaffs()
//        {
//            var staffDTOs = (from staff in _db.Staffs
//                             join user in _db.Users on staff.UserID equals user.UserID
//                             join account in _db.Accounts on user.AccountID equals account.AccountID
//                             select new GetStaffDTO
//                             {
//                                 StaffID = staff.StaffID,
//                                 FullName = user.FullName,
//                                 Email = account.Email,
//                                 PhoneNumber = user.PhoneNumber,
//                                 IdentityCard = user.IdentityCard,
//                                 Gender = user.Gender,
//                                 BirthDate = user.BirthDate,
//                                 Address = user.Address,
//                                 Position = staff.Position,
//                                 Salary = staff.Salary
//                             }).ToList();

//            return View(staffDTOs);
//        }

//        public async Task<int> GetWardIDAsync(string wardName, string districtName, string provinceName)
//        {
//            var ward = await _db.Wards
//                .Include(w => w.District)
//                    .ThenInclude(d => d.Province)
//                .FirstOrDefaultAsync(w => w.WardName == wardName &&
//                                            w.District.DistrictName == districtName &&
//                                            w.District.Province.ProvinceName == provinceName);
//                return ward.WardID;
//        }
//        public JsonResult GetProvince()
//        {
//            var province = _db.Provinces.ToList();
//            return new JsonResult(province);
//        }
//        public JsonResult GetDistrict(int id)
//        {
//            var district = _db.Districts.Where(x => x.Province.ProvinceID == id).ToList();
//            return new JsonResult(district);
//        }
//        public JsonResult GetWard(int id)
//        {
//            var ward = _db.Wards.Where(x => x.District.DistrictID == id).ToList();
//            return new JsonResult(ward);
//        }

//        public async Task<IActionResult> AddStaff(AddStaffDTO staffDTO)
//        {
//            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
//            {
//                Console.WriteLine($"Error: {error.ErrorMessage}");
//            }
//            if (ModelState.IsValid)
//            {
//                using (var transaction = _db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        int newRoleID = 2;
//                        //int wardID = await GetWardIDAsync(staffDTO.WardName, staffDTO.DistrictName, staffDTO.ProvinceName);
//                        var accid = 1;
//                        var lastAcc = _db.Accounts.OrderByDescending(a => a.AccountID).FirstOrDefault();
//                        if (lastAcc != null)
//                        {
//                            accid = Convert.ToInt32(lastAcc.AccountID) + 1;
//                        }
//                        Console.WriteLine(accid + staffDTO.Email);
//                        var accidTxt = accid.ToString().PadLeft(8, '0');
//                        var newAccount = new Account
//                        {
//                            AccountID = accidTxt,
//                            Email = staffDTO.Email,
//                            Password = "123456",
//                            RoleID = newRoleID,
//                        };
//                        _db.Accounts.Add(newAccount);
//                        await _db.SaveChangesAsync();

//                        var userid = 1;
//                        var lastUser = _db.Users.OrderByDescending(u => u.UserID).FirstOrDefault();
//                        if (lastUser != null)
//                        {
//                            userid = Convert.ToInt32(lastUser.UserID.Substring(2)) + 1;
//                        }
//                        var useridTxt = "NV" + userid.ToString().PadLeft(6, '0');
//                        int newWardID = await GetWardIDAsync(staffDTO.WardName, staffDTO.DistrictName, staffDTO.ProvinceName);
//                        var newUser = new User
//                        {
//                            UserID = useridTxt,
//                            FullName = staffDTO.FullName,
//                            PhoneNumber = staffDTO.PhoneNumber,
//                            IdentityCard = staffDTO.IdentityCard,
//                            Gender = staffDTO.Gender,
//                            BirthDate = staffDTO.BirthDate,
//                            Address = staffDTO.Address,
//                            Photo = staffDTO.Photo,
//                            WardID = newWardID,
//                            AccountID = newAccount.AccountID,
//                        };
//                        _db.Users.Add(newUser);
//                        await _db.SaveChangesAsync();

//                        var staffId = 1;
//                        var lastStaff = _db.Staffs.OrderByDescending(u => u.StaffID).FirstOrDefault();
//                        if (lastStaff != null)
//                        {
//                            staffId = Convert.ToInt32(lastStaff.UserID.Substring(2)) + 1;
//                        }
//                        var staffTxt = "NV" + staffId.ToString().PadLeft(6, '0');
//                        staffDTO.StaffID = staffTxt;
//                        var newStaff = new Staff
//                        {
//                            StaffID = staffDTO.StaffID,
//                            Position = staffDTO.Position,
//                            Salary = staffDTO.Salary,
//                            UserID = newUser.UserID,
//                        };
//                        _db.Staffs.Add(newStaff);
//                        await _db.SaveChangesAsync();
//                        transaction.Commit(); 
//                        return RedirectToAction("Index", "Home");

//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
//                        ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi thêm nhân viên: " + errorMessage);
//                    }
//                }
//            }
//            //var provinces = await _db.Provinces.ToListAsync();
//            //ViewBag.Provinces = provinces;
//            return View(staffDTO);
//        }
//        public async Task<IActionResult> DeleteStaff(string id)
//        {
//            if (String.IsNullOrEmpty(id))
//            {
//                return NotFound();
//            }
//            Staff staff = await _db.Staffs.FindAsync(id);
//            if (staff != null)
//            {
//                _db.Staffs.Remove(staff);
//            }
//            User user = await _db.Users.FindAsync(staff.UserID);
//            if (user != null)
//            {
//                _db.Users.Remove(user);
//            }
//            Account account = await _db.Accounts.FindAsync(user.AccountID);
//            if (user != null)
//            {
//                _db.Accounts.Remove(account);
//            }
//            await _db.SaveChangesAsync();

//            return RedirectToAction("listStaffs");
//        }

//        public IActionResult Details(string? id)
//        {
//            if (String.IsNullOrEmpty(id))
//            {
//                return NotFound();
//            }
//            Staff? staff = _db.Staffs.Find(id);

//            if (staff == null)
//            {
//                return NotFound();
//            }
//            User user = _db.Users.FirstOrDefault(u => u.UserID == staff.UserID);
//            Account account = _db.Accounts.FirstOrDefault(a => a.AccountID == user.AccountID);
//            GetStaffDTO staffDTO = new GetStaffDTO
//            {
//                StaffID = staff.StaffID,
//                FullName = user.FullName,
//                Email = account.Email,
//                PhoneNumber = user.PhoneNumber,
//                IdentityCard = user.IdentityCard,
//                Gender = user.Gender,
//                BirthDate = user.BirthDate,
//                Address = user.Address,
//                Photo = user.Photo,
//                Position = staff.Position,
//                Salary = staff.Salary,
//            };
//            return View(staffDTO);
//        }

//        public async Task<ActionResult> UpdateStaff(int id, UpdateStaffDTO staffDTO)
//        {

//            try
//            {
//                var staff = await _db.Staffs.FindAsync(id);
//                var user = await _db.Users.FindAsync(staff.UserID);
//                var account = await _db.Accounts.FindAsync(user.AccountID);
//                var ward = await _db.Wards.FindAsync(user.WardID);
//                var district = await _db.Districts.FindAsync(ward.DistrictID);
//                var province = await _db.Provinces.FindAsync(district.ProvinceID);
//                if (staff != null)
//                {
//                    staff.Position = staffDTO.Position;
//                    staff.Salary = (int)staffDTO.Salary;
//                    user.FullName = staffDTO.FullName;
//                    account.Email = staffDTO.Email;
//                    user.Address = staffDTO.Address;
//                    user.PhoneNumber = staffDTO.PhoneNumber;
//                    user.IdentityCard = staffDTO.IdentityCard;
//                    user.Gender = staffDTO.Gender;
//                    user.BirthDate = staffDTO.BirthDate;
//                    user.Photo = staffDTO.Photo;
//                    ward.WardName = staffDTO.WardName;
//                    district.DistrictName = staffDTO.DistrictName;
//                    province.ProvinceName = staffDTO.ProvinceName;
//                    int newWardID = await GetWardIDAsync(staffDTO.WardName, staffDTO.DistrictName, staffDTO.ProvinceName);
//                    user.WardID = newWardID;
//                    await _db.SaveChangesAsync();
//                    var getStaff = new GetStaffDTO
//                    {
//                        StaffID = staff.StaffID,
//                        UserID = user.UserID,
//                        AccountID = user.AccountID,
//                        RoleID = account.RoleID,
//                        FullName = user.FullName,
//                        Email = account.Email,
//                        PhoneNumber = user.PhoneNumber,
//                        IdentityCard = user.IdentityCard,
//                        Gender = user.Gender,
//                        BirthDate = user.BirthDate,
//                        Address = user.Address,
//                        Photo = user.Photo,
//                        Position = staff.Position,
//                        Salary = staff.Salary,
//                        WardID = user.WardID,
//                    };
//                    return StatusCode(StatusCodes.Status200OK, getStaff);

//                }
//                else
//                {
//                    return StatusCode(StatusCodes.Status404NotFound, "Không tìm thấy ID!");
//                }
//            }
//            catch (Exception err)
//            {
//                return StatusCode(StatusCodes.Status400BadRequest, err);
//            }
//        }
//        public async Task<ActionResult<Car>> getStaffById([FromRoute] string id)
//        {
//            try
//            {
//                var staff = await _db.Staffs.FindAsync(id);
//                if (staff != null)
//                {
//                    return StatusCode(StatusCodes.Status200OK, staff);
//                }
//                else
//                    return StatusCode(StatusCodes.Status400BadRequest, "Không tồn tại ID!");
//            }
//            catch (Exception err)
//            {
//                return StatusCode(StatusCodes.Status400BadRequest, err);
//            }
//        }
//        public IActionResult Search(string searchTerm, string searchField)
//        {
//            IQueryable<GetStaffDTO> staffQuery = _db.Staffs
//                .Include(s => s.User)
//                .Include(s => s.User.Account)
//                .Select(s => new GetStaffDTO
//                {
//                    StaffID = s.StaffID,
//                    FullName = s.User.FullName,
//                    Email = s.User.Account.Email,
//                    PhoneNumber = s.User.PhoneNumber,
//                    IdentityCard = s.User.IdentityCard,
//                    Gender = s.User.Gender,
//                    BirthDate = s.User.BirthDate,
//                    Address = s.User.Address,
//                    Position = s.Position,
//                    Salary = s.Salary
//                });

//            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchField))
//            {
//                switch (searchField)
//                {
//                    case "FullName":
//                        staffQuery = staffQuery.Where(s => s.FullName.Contains(searchTerm));
//                        break;
//                    case "IdentityCard":
//                        staffQuery = staffQuery.Where(s => s.IdentityCard.Contains(searchTerm));
//                        break;
//                    case "PhoneNumber":
//                        staffQuery = staffQuery.Where(s => s.PhoneNumber.Contains(searchTerm));
//                        break;
//                    case "Address":
//                        staffQuery = staffQuery.Where(s => s.Address.Contains(searchTerm));
//                        break;
//                }
//            }

//            var staffDTOs = staffQuery.ToList();
//            return View("listStaffs", staffDTOs);
//        }

//    }
//}
