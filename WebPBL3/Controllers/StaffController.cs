using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPBL3.DTO.Staff;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class StaffController : Controller
    {
        private ApplicationDbContext _db;

        public StaffController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult listStaffs()
        {
            var staffDTOs = (from staff in _db.Staffs
                             join user in _db.Users on staff.UserID equals user.UserID
                             select new GetStaffDTO
                             {
                                 StaffID = staff.StaffID,
                                 FullName = user.FullName,
                                 PhoneNumber = user.PhoneNumber,
                                 IdentityCard = user.IdentityCard,
                                 Gender = user.Gender,
                                 BirthDate = user.BirthDate,
                                 Address = user.Address,
                                 Position = staff.Position,
                                 Salary = staff.Salary
                             }).ToList();

            return View(staffDTOs);
        }

        public async Task<int> GetWardIDAsync(string wardName, string districtName, string provinceName)
        {
            var ward = await _db.Wards
                .Include(w => w.District)
                    .ThenInclude(d => d.Province)
                .FirstOrDefaultAsync(w => w.WardName == wardName &&
                                            w.District.DistrictName == districtName &&
                                            w.District.Province.ProvinceName == provinceName);
            return ward.WardID;
        }
        public IActionResult GetDistricts(string provinceName)
        {
            var districts = _db.Districts.Where(d => d.Province.ProvinceName == provinceName)
                                          .Select(d => new { d.DistrictID, d.DistrictName })
                                          .ToList();
            return Json(districts);
        }

        public IActionResult GetWards(string provinceName, string districtName)
        {
            var wards = _db.Wards.Where(w => w.District.DistrictName == districtName &&
                                              w.District.Province.ProvinceName == provinceName)
                                 .Select(w => new { w.WardID, w.WardName })
                                 .ToList();
            return Json(wards);
        }

        public async Task<string> GenerateUserIDAsync()
        {
            int userCount = await _db.Users.CountAsync();
            string newUserID = (userCount + 1).ToString("D10");
            return newUserID;
        }
        public async Task<string> GenerateStaffIDAsync()
        {
            int staffCount = await _db.Staffs.CountAsync();
            string newStaffID = (staffCount + 1).ToString("D10");
            return newStaffID;
        }
        public async Task<string> GenerateAccountIDAsync()
        {
            int count = await _db.Accounts.CountAsync();
            string newAcountID = (count + 1).ToString("D10");
            return newAcountID;
        }

        public async Task<IActionResult> AddStaff(AddStaffDTO staffDTO)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error: {error.ErrorMessage}");
            }
            if (ModelState.IsValid)
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        string newUserID = await GenerateUserIDAsync();
                        string newAcountID = await GenerateUserIDAsync();
                        string newStaffID = await GenerateStaffIDAsync();
                        int newRoleID = 2;
                        //int wardID = await GetWardIDAsync(staffDTO.WardName, staffDTO.DistrictName, staffDTO.ProvinceName);
                        var newAccount = new Account
                        {
                            AccountID = newAcountID,
                            Email = staffDTO.Email,
                            Password = newAcountID,
                            RoleID = newRoleID,
                        };
                        _db.Accounts.Add(newAccount);
                        await _db.SaveChangesAsync();
                        var newUser = new User
                        {
                            UserID = newUserID,
                            FullName = staffDTO.FullName,
                            PhoneNumber = staffDTO.PhoneNumber,
                            IdentityCard = staffDTO.IdentityCard,
                            Gender = staffDTO.Gender,
                            BirthDate = staffDTO.BirthDate,
                            Address = staffDTO.Address,
                            Photo = staffDTO.Photo,
                            WardID = staffDTO.WardID,
                            AccountID = newAccount.AccountID,
                        };
                        _db.Users.Add(newUser);
                        await _db.SaveChangesAsync();
                        var newStaff = new Staff
                        {
                            StaffID = newStaffID,
                            Position = staffDTO.Position,
                            Salary = staffDTO.Salary,
                            UserID = newUser.UserID,
                        };
                        _db.Staffs.Add(newStaff);
                        await _db.SaveChangesAsync();
                        transaction.Commit(); 
                        return RedirectToAction("Index", "Home");

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi thêm nhân viên: " + errorMessage);
                    }
                }
            }
            return View(staffDTO);
        }
        public async Task<IActionResult> DeleteStaff(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var staff = await _db.Staffs
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.StaffID == id);
            if (staff == null)
            {
                return NotFound();
            }

            _db.Staffs.Remove(staff);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> UpdateStaff(int id, AddStaffDTO staffDTO)
        {
            try
            {
                var staff = await _db.Staffs.FindAsync(id);
                var user = await _db.Users.FindAsync(staff.UserID);
                var ward = await _db.Wards.FindAsync(user.WardID);
                //var district = await _db.Districts.FindAsync(ward.DistrictID);
                //var province = await _db.Provinces.FindAsync(district.ProvinceID);
                //int newWardID;
                if (staff != null)
                {
                    staff.StaffID = staffDTO.StaffID;
                    staff.Position = staffDTO.Position;
                    staff.Salary = staffDTO.Salary;
                    user.FullName = staffDTO.FullName;
                    user.Address = staffDTO.Address;
                    user.PhoneNumber = staffDTO.PhoneNumber;
                    user.IdentityCard = staffDTO.IdentityCard;
                    user.Gender = staffDTO.Gender;
                    user.BirthDate = staffDTO.BirthDate;
                    user.Photo = staffDTO.Photo;
                    //ward.WardName = staffDTO.WardName;
                    //district.DistrictName = staffDTO.DistrictName;
                    //province.ProvinceName = staffDTO.ProvinceName;
                    //newWardID = await GetWardIDAsync(staffDTO.WardName, staffDTO.DistrictName, staffDTO.ProvinceName);
                    user.WardID = staffDTO.WardID;
                    await _db.SaveChangesAsync();
                    var getStaff = new GetStaffDTO
                    {
                        StaffID = staff.StaffID,
                        FullName = user.FullName,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        IdentityCard = user.IdentityCard,
                        Gender = user.Gender,
                        BirthDate = user.BirthDate,
                        Photo = user.Photo,
                        Salary = staff.Salary,
                        Position = staff.Position,
                        WardID = user.WardID,
                    };
                    return StatusCode(StatusCodes.Status200OK, getStaff);

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Không tìm thấy ID!");
                }
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err);
            }
        }
        public async Task<ActionResult<Car>> getStaffById([FromRoute] string id)
        {
            try
            {
                var staff = await _db.Staffs.FindAsync(id);
                if (staff != null)
                {
                    return StatusCode(StatusCodes.Status200OK, staff);
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Không tồn tại ID!");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err);
            }
        }
    }
}
