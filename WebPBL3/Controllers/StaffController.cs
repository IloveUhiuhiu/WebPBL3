using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebPBL3.Models;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OfficeOpenXml;
using WebPBL3.DTO;
using Microsoft.AspNetCore.Authorization;
using WebPBL3.Services;

namespace WebPBL3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StaffController : Controller
    {
        private ApplicationDbContext _db;
		private int limits = 10;
		private readonly IWebHostEnvironment environment;
        private readonly IStaffService _staffService;
        IUserService _userService;
        IPhotoService _photoService;
        public StaffController(ApplicationDbContext db, IWebHostEnvironment environment, IStaffService staffService, IUserService userService, IPhotoService photoService)
        {
            _db = db;
            this.environment = environment;
            _staffService = staffService;
            _userService = userService;
            _photoService = photoService;
        }
        public async Task<IActionResult> listStaffs(int page = 1)
        {
            var staffDTOs = await _staffService.GetAllStaffs();
            int limits = 10; 
            var total = staffDTOs.Count();
            var totalPage = (total + limits - 1) / limits;
            if (page < 1) page = 1;
            if (page > totalPage) page = totalPage;

            var paginatedStaffDTOs = staffDTOs.Skip((page - 1) * limits).Take(limits).ToList();

            ViewBag.staffs = paginatedStaffDTOs;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = page;

            return View(paginatedStaffDTOs);
        }

        [HttpGet]
        public JsonResult GetProvince()
        {
            var provinces = _db.Provinces.ToList();
            return new JsonResult(provinces);
        }

        public JsonResult GetDistrict(int id)
        {
            var districts = _db.Districts.Where(d => d.ProvinceID == id).Select(d => new { id = d.DistrictID, name = d.DistrictName }).ToList();
            return new JsonResult(districts);
        }

        public JsonResult GetWard(int id)
        {
            var wards = _db.Wards.Where(w => w.DistrictID == id).Select(w => new { id = w.WardID, name = w.WardName }).ToList();
            return new JsonResult(wards);
        }
        public IActionResult AddStaff()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStaff(StaffDTO staffDTO, IFormFile? uploadimage)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    //try
                    //{
                        int newRoleID = 2;
                        staffDTO.RoleID = newRoleID;
                        bool checkEmailExist = await _userService.CheckEmailExits(staffDTO.Email);
                        if (checkEmailExist)
                        {
                            TempData["Error"] = "Email đã tồn tại";
                            return View(staffDTO);
                        }
                       staffDTO.UserID = await _userService.GenerateID();
                        bool checkIdentityCardExist = await _staffService.CheckIdentityCardExits(staffDTO.IdentityCard);
                        staffDTO.StaffID = await _staffService.GenerateID();
                        if (uploadimage != null && uploadimage.Length > 0)
                        {
                            staffDTO.Photo = await _photoService.AddPhoto("staff", uploadimage);

                        }
                        try
                        {

                            await _staffService.AddStaff(staffDTO);
                            
                        }
                        catch (DbUpdateException ex)
                        {
                            return BadRequest("Error add user: " + ex.Message);
                        }
                        return RedirectToAction("listStaffs");
                    //{
                    //var accid = 1;
                    //var lastAcc = _db.Accounts.OrderByDescending(a => a.AccountID).FirstOrDefault();
                    //if (lastAcc != null)
                    //{
                    //    accid = Convert.ToInt32(lastAcc.AccountID) + 1;
                    //}
                    //var accidTxt = accid.ToString().PadLeft(8, '0');
                    //var accWithEmail = _db.Accounts.FirstOrDefault(u => u.Email == staffDTO.Email);
                    //if (accWithEmail != null)
                    //{
                    //    TempData["Error"] = "Email đã tồn tại";
                    //    return View(staffDTO);
                    //}
                    //staffDTO.AccountID = accidTxt;
                    //var newAccount = _staffService.ConvertToAccount(staffDTO, newRoleID, BCrypt.Net.BCrypt.HashPassword("123456"));

                    //_db.Accounts.Add(newAccount);
                    //await _db.SaveChangesAsync();

                    //var userid = 1;
                    //var lastUser = _db.Users.OrderByDescending(u => u.UserID).FirstOrDefault();
                    //if (lastUser != null)
                    //{
                    //    userid = Convert.ToInt32(lastUser.UserID.Substring(2)) + 1;
                    //}
                    //var useridTxt = "NV" + userid.ToString().PadLeft(6, '0');
                    //staffDTO.UserID = useridTxt;

                    //var accWithIdentityCard = _db.Users.FirstOrDefault(u => u.IdentityCard == staffDTO.IdentityCard);
                    //if (accWithIdentityCard != null)
                    //{
                    //    TempData["Error"] = "CCCD đã tồn tại";
                    //    return View(staffDTO);
                    //}
                    //string? newFilename = null;
                    //if (staffDTO.Photo != null)
                    //{
                    //    newFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    //    newFilename += Path.GetExtension(staffDTO.Photo!.FileName);
                    //    string imageFullPath = environment.WebRootPath + "/upload/staff/" + newFilename;
                    //    using (var stream = System.IO.File.Create(imageFullPath))
                    //    {
                    //        staffDTO.Photo.CopyTo(stream);
                    //    }
                    //}
                    //var newUser = _staffService.ConvertToUser(staffDTO, newFilename);

                    //_db.Users.Add(newUser);
                    //await _db.SaveChangesAsync();
                    //var staffId = 1;
                    //var lastStaff = _db.Staffs.OrderByDescending(u => u.StaffID).FirstOrDefault();
                    //if (lastStaff != null)
                    //{
                    //    staffId = Convert.ToInt32(lastStaff.StaffID.Substring(2)) + 1;
                    //}
                    //var staffTxt = "NV" + staffId.ToString().PadLeft(6, '0');
                    //staffDTO.StaffID = staffTxt;

                    //var newStaff = _staffService.ConvertToStaff(staffDTO);
                    //_db.Staffs.Add(newStaff);
                    //await _db.SaveChangesAsync();
                    //transaction.Commit();
                    //return RedirectToAction("listStaffs");

                    //}
                    //catch (Exception ex)
                    //{
                    //transaction.Rollback();
                    //var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    //ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi thêm nhân viên: " + errorMessage);
                    //}

                }
            }
            return View(staffDTO);
        }
        public async Task<IActionResult> DeleteStaff(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Staff? staff = await _db.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            User? user = await _db.Users.FindAsync(staff.UserID);
            if (user == null)
            {
                return NotFound();
            }
            Account? account = await _db.Accounts.FindAsync(user.AccountID);
            if (account == null)
            {
                return NotFound();
            }
            _db.Accounts.Remove(account);
            await _db.SaveChangesAsync();

            return RedirectToAction("listStaffs");
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Staff? staff = _db.Staffs.Find(id);
            if (staff == null)
            {
                return NotFound();
            }
            User? user = _db.Users.FirstOrDefault(u => u.UserID == staff.UserID);
            if(user == null)
            {
                return NotFound();
            }    
            Account? account = _db.Accounts.FirstOrDefault(a => a.AccountID == user.AccountID);
            if (account == null)
            {
                return NotFound();
            }
            Ward? ward = _db.Wards.FirstOrDefault(w => w.WardID == user.WardID);
            District? district = null;
            Province? province = null;

            if (ward != null)
            {
                district =  _db.Districts.FirstOrDefault(d => d.DistrictID == ward.DistrictID);
                province =  _db.Provinces.FirstOrDefault(p => p.ProvinceID == district.ProvinceID);               
            }

            string? districtName = district?.DistrictName;
            string? provinceName = province?.ProvinceName;
            string? wardName = ward?.WardName;

            StaffDTO staffDTO = await _staffService.ConvertToStaffDTO(staff, user, account);
            ViewData["Photo"] = user.Photo;
            return View(staffDTO);
        }

        public async Task<ActionResult> UpdateStaff(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Staff? staff = await _db.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == staff.UserID);
            if(user == null)
            {
                return NotFound();
            }    
            Account? account = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountID == user.AccountID);
            if(account == null)
            {
                return NotFound();
            }    
            Ward? ward = await _db.Wards.FirstOrDefaultAsync(w => w.WardID == user.WardID);
            District? district = null;
            Province? province = null;

            if (ward != null)
            {
                district = await _db.Districts.FirstOrDefaultAsync(d => d.DistrictID == ward.DistrictID);
                province = await _db.Provinces.FirstOrDefaultAsync(p => p.ProvinceID == district.ProvinceID);
            }

            string? districtName = district?.DistrictName;
            string? provinceName = province?.ProvinceName;
            string? wardName = ward?.WardName;
            int? districtID = district?.DistrictID;
            int? provinceID = province?.ProvinceID;
            int? wardID = ward?.WardID;
            StaffDTO getstaffDTO = await _staffService.ConvertToStaffDTO(staff, user, account);

            ViewBag.ProvinceName = provinceName;
            ViewBag.DistrictName = districtName;
            ViewBag.WardName = wardName;
            ViewBag.ProvinceID = provinceID;
            ViewBag.DistrictID = districtID;
            ViewBag.WardID = wardID;
            ViewData["Photo"] = user.Photo;
            return View("UpdateStaff",getstaffDTO);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStaff(StaffDTO staffDTO)
        {
            //Staff? staff = await _db.Staffs.AsNoTracking().FirstOrDefaultAsync(s => s.StaffID == staffDTO.StaffID);
            //if (staff == null)
            //{
            //    return NotFound();
            //}
            //User? user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == staff.UserID);
            //if (user == null)
            //{
            //    return NotFound();
            //}
            //Ward? ward = await _db.Wards.FirstOrDefaultAsync(w => w.WardID == user.WardID);
            //District? district = null;
            //Province? province = null;

            //if (ward != null)
            //{
            //    district = await _db.Districts.FirstOrDefaultAsync(d => d.DistrictID == ward.DistrictID);
            //    province = await _db.Provinces.FirstOrDefaultAsync(p => p.ProvinceID == district.ProvinceID);
            //}

            //// Kiểm tra nếu district hoặc province là null để tránh lỗi
            //string? districtName = district?.DistrictName;
            //string? provinceName = province?.ProvinceName;
            //string? wardName = ward?.WardName;
            //int? districtID = district?.DistrictID;
            //int? provinceID = province?.ProvinceID;
            //int? wardID = ward?.WardID;
            //Account? account = await _db.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.AccountID == user.AccountID);
            //if (account == null)
            //{
            //    return NotFound();
            //}    
            //string? newFilename = user.Photo;
            //if (ModelState.IsValid)
            //{
            //    if(staffDTO.Photo != null)
            //    {
            //        newFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //        newFilename += Path.GetExtension(staffDTO.Photo!.FileName);
            //        string imageFullPath = environment.WebRootPath + "/upload/staff/" + newFilename;
            //        using (var stream = System.IO.File.Create(imageFullPath))
            //        {
            //            staffDTO.Photo.CopyTo(stream);
            //        }
            //        if (!string.IsNullOrEmpty(user.Photo))
            //        {
            //            string oldImageFullPath = environment.WebRootPath + "/upload/staff/" + user.Photo;
            //            if (System.IO.File.Exists(oldImageFullPath))
            //            {
            //                System.IO.File.Delete(oldImageFullPath);
            //            }
            //        }
            //    }

            //    staffDTO.StaffID = staff.StaffID;
            //    staffDTO.UserID = user.UserID;
            //    staffDTO.AccountID = account.AccountID;
            //    staffDTO.WardID = Convert.ToInt32(staffDTO.WardName);
            //    staff = _staffService.ConvertToStaff(staffDTO);
            //    user = _staffService.ConvertToUser(staffDTO, newFilename);
            //    account = _staffService.ConvertToAccount(staffDTO, account.RoleID, account.Password);
            //    try
            //    {
            //        _db.Entry(staff).State = EntityState.Modified;
            //        _db.Entry(user).State = EntityState.Modified;
            //        _db.Entry(account).State = EntityState.Modified;
            //        await _db.SaveChangesAsync();
            //        return RedirectToAction("listStaffs");
            //    }
            //    catch (DbUpdateConcurrencyException ex)
            //    {
            //        return NotFound();
            //    }

            //}
            //ViewBag.ProvinceName = provinceName;
            //ViewBag.DistrictName = districtName;
            //ViewBag.WardName = wardName;
            //ViewBag.ProvinceID = provinceID;
            //ViewBag.DistrictID = districtID;
            //ViewBag.WardID = wardID;
            //ViewData["Photo"] = user.Photo;
            return View(staffDTO);

        }


        public async Task<IActionResult> Search(string searchTerm, string searchField, int page = 1)
        {
            page = page < 1 ? 1 : page;

            var staffQuery = await _staffService.GetStaffsBySearch(searchTerm, searchField);
            var staffs = staffQuery.ToList();
            if (staffs.Any())
            {
                ViewBag.staffs = staffs;
            }
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SearchField = searchField;

            var total = staffs.Count;
            var totalPage = (total + limits - 1) / limits;
            if (page < 1) page = 1;
            if (page > totalPage) page = totalPage;
            int offset = (page - 1) * limits;
            if (offset < 0) offset = 0;
            ViewBag.totalRecord = total;
            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = page;

            var staffDTOs = staffQuery
                .OrderBy(staff => staff.StaffID)
                .Skip(offset)
                .Take(limits)
                .ToList();
            return View("listStaffs", staffDTOs);
        }

        [HttpPost]
        public IActionResult SaveExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var staffTable = JsonConvert.DeserializeObject<List<StaffDTO>>(Request.Form["data"], new JsonSerializerSettings { MaxDepth = 10 });
            if (staffTable == null || !staffTable.Any())
            {
                TempData["Error"] = "Chưa có dữ liệu";
                return RedirectToAction("listStaffs");
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã nhân viên";
                worksheet.Cells[1, 3].Value = "Tên nhân viên";
                worksheet.Cells[1, 4].Value = "Giới tính";
                worksheet.Cells[1, 5].Value = "Số điện thoại";
                worksheet.Cells[1, 6].Value = "CCCD";
                worksheet.Cells[1, 7].Value = "Email";
                worksheet.Cells[1, 8].Value = "Địa chỉ";
                worksheet.Cells[1, 9].Value = "Chức vụ";
                worksheet.Cells[1, 10].Value = "Lương";
                for(int i = 0; i < staffTable.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = i + 1;
                    worksheet.Cells[i + 2, 2].Value = staffTable[i].StaffID;
                    worksheet.Cells[i + 2, 3].Value = staffTable[i].FullName;
                    if (staffTable[i].Gender.HasValue)
                    {
                        if (staffTable[i].Gender == true)
                        {
                            worksheet.Cells[i + 2, 4].Value = "Nam";
                        }
                        else
                        {
                            worksheet.Cells[i + 2, 4].Value = "Nữ";
                        }
                    }
                    else
                    {
                        worksheet.Cells[i + 2, 4].Value = "Không xác định";
                    }
                    worksheet.Cells[i + 2, 5].Value = staffTable[i].PhoneNumber;
                    worksheet.Cells[i + 2, 6].Value = staffTable[i].IdentityCard;
                    worksheet.Cells[i + 2, 7].Value = staffTable[i].Email;
                    worksheet.Cells[i + 2, 8].Value = staffTable[i].Address;
                    worksheet.Cells[i + 2, 9].Value = staffTable[i].Position;
                    worksheet.Cells[i + 2, 10].Value = staffTable[i].Salary;
                }
                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);
                memoryStream.Position = 0;
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "file.xlsx");
            }
        }

	}
}
