using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebPBL3.DTO;
using WebPBL3.Models;
namespace WebPBL3.Services
{
    public class StaffService : IStaffService
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment _environment;
        IUserService _userService;
        public StaffService(ApplicationDbContext db, IWebHostEnvironment environment, IUserService userService)
        {
            _db = db;
            _environment = environment;
            _userService = userService; 
        }
        public Task<List<StaffDTO>> GetAllStaffs()
        {
            var staffs = (from staff in _db.Staffs
                             join user in _db.Users on staff.UserID equals user.UserID
                             join ward in _db.Wards on user.WardID equals ward.WardID
                             join district in _db.Districts on ward.DistrictID equals district.DistrictID
                             join province in _db.Provinces on district.ProvinceID equals province.ProvinceID
                             join account in _db.Accounts on user.AccountID equals account.AccountID
                             select new StaffDTO
                             {
                                 StaffID = staff.StaffID,
                                 FullName = user.FullName,
                                 Email = account.Email,
                                 PhoneNumber = user.PhoneNumber,
                                 IdentityCard = user.IdentityCard,
                                 Gender = user.Gender,
                                 BirthDate = user.BirthDate,
                                 Address = user.Address,
                                 Position = staff.Position,
                                 Salary = staff.Salary,
                                 Status = account.Status,
                                 WardName = ward.WardName,
                                 ProvinceName = province.ProvinceName,
                                 DistrictName = district.DistrictName
                             }).ToListAsync();
            return staffs;
        }
        public async Task<IEnumerable<StaffDTO>> GetStaffsBySearch(string searchTerm, string searchField)
        {
            var staffQuery = from staff in _db.Staffs
                             join user in _db.Users on staff.UserID equals user.UserID
                             join ward in _db.Wards on user.WardID equals ward.WardID
                             join district in _db.Districts on ward.DistrictID equals district.DistrictID
                             join province in _db.Provinces on district.ProvinceID equals province.ProvinceID
                             join account in _db.Accounts on user.AccountID equals account.AccountID
                             select new StaffDTO
                             {
                                 StaffID = staff.StaffID,
                                 FullName = user.FullName,
                                 Email = account.Email,
                                 PhoneNumber = user.PhoneNumber,
                                 IdentityCard = user.IdentityCard,
                                 Gender = user.Gender,
                                 BirthDate = user.BirthDate,
                                 Address = user.Address,
                                 Position = staff.Position,
                                 Salary = staff.Salary,
                                 ProvinceName = province.ProvinceName,
                                 DistrictName = district.DistrictName,
                                 WardName = ward.WardName
                             };

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchField))
            {
                switch (searchField)
                {
                    case "FullName":
                        staffQuery = staffQuery.Where(s => s.FullName != null && s.FullName.Contains(searchTerm));
                        break;
                    case "IdentityCard":
                        staffQuery = staffQuery.Where(s => s.IdentityCard != null && s.IdentityCard.Contains(searchTerm));
                        break;
                    case "PhoneNumber":
                        staffQuery = staffQuery.Where(s => s.PhoneNumber != null && s.PhoneNumber.Contains(searchTerm));
                        break;
                    case "Address":
                        staffQuery = staffQuery.Where(s => s.Address != null && s.Address.Contains(searchTerm));
                        break;
                }
            }
            return staffQuery;
        }
        public async Task<Staff> GetStaffById(string id)
        {
            try
            {
                Staff? staff = await _db.Staffs.FirstOrDefaultAsync(u => u.StaffID == id);
                return staff;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xảy ra khi truy vấn người dùng: ", ex);
            }
        }
        public async Task AddStaff(StaffDTO staffDTO)
        {
            try
            {
                await _userService.AddUser(staffDTO);
                Staff staff = ConvertToStaff(staffDTO);
                _db.Staffs.Add(staff);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xuất hiện khi thêm người dùng: ", ex);
            }
        }
        public async Task EditStaff(Staff staff)
        {
            try
            {
                _db.Staffs.Update(staff);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xuất hiện khi cập nhật người dùng: ", ex);
            }
        }
        public async Task DeleteStaff(Staff staff)
        {
            try
            {
                _db.Staffs.Remove(staff);
                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xảy ra khi xóa người dùng: ", ex);
            }
        }

        public Staff ConvertToStaff(StaffDTO StaffDTO)
        {
            try
            {
                Staff staff = new Staff
                {
                    StaffID = StaffDTO.StaffID,
                    Position = StaffDTO.Position,
                    Salary = StaffDTO.Salary,
                    UserID = StaffDTO.UserID
                };
                return staff;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi StaffDTO thành Staff: ", ex);
            }
        }
        public User ConvertToUser(StaffDTO StaffDTO, string? photo)
        {
            try
            {
                User user = new User
                {
                    UserID = StaffDTO.UserID,
                    FullName = StaffDTO.FullName,
                    PhoneNumber = StaffDTO.PhoneNumber,
                    IdentityCard = StaffDTO.IdentityCard,
                    Gender = StaffDTO.Gender,
                    Address = StaffDTO.Address,
                    BirthDate = StaffDTO.BirthDate,
                    Photo = photo,
                    WardID = Convert.ToInt32(StaffDTO.WardName),
                    AccountID = StaffDTO.AccountID,
                };
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi StaffDTO thành Staff: ", ex);
            }
        }
        public Account ConvertToAccount(StaffDTO StaffDTO, int roleId, string hashedPassword)
        {
            try
            {
                Account account = new Account
                {
                    AccountID = StaffDTO.AccountID,
                    Email = StaffDTO.Email,
                    Status = false,
                    Password = hashedPassword,
                    RoleID = roleId,
                };
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi StaffDTO thành Staff: ", ex);
            }
        }
        public async Task<string> GenerateID()
        {
            int staffId = 1;
            Staff? lastUser = await _db.Staffs.OrderByDescending(u => u.StaffID).FirstOrDefaultAsync();
            if (lastUser != null)
            {
                staffId = Convert.ToInt32(lastUser.StaffID.Substring(2)) + 1;
            }
            string staffIdTxt = "NV" + staffId.ToString().PadLeft(6, '0');
            return staffIdTxt;
        }
        public async Task<bool> CheckIdentityCardExits(string? identityCard)
        {
            try
            {
                User? user = await _db.Users.FirstOrDefaultAsync(a => a.IdentityCard == identityCard);
                return (user != null);
            }

            catch (Exception ex)
            {
                throw new Exception("Lỗi xảy ra khi truy vấn tài khoản: ", ex);
            }
        }
        public Task<StaffDTO> ConvertToStaffDTO(Staff staff, User user, Account account)
        {
            try
            {
                Ward? ward = _db.Wards.FirstOrDefault(w => w.WardID == user.WardID);
                District? district = null;
                Province? province = null;

                if (ward != null)
                {
                    district = _db.Districts.FirstOrDefault(d => d.DistrictID == ward.DistrictID);
                    province = _db.Provinces.FirstOrDefault(p => p.ProvinceID == district.ProvinceID);
                }
                string? districtName = district?.DistrictName;
                string? provinceName = province?.ProvinceName;
                string? wardName = ward?.WardName;
                StaffDTO staffDto = new StaffDTO
                {
                    StaffID = staff.StaffID,
                    FullName = user.FullName,
                    Email = account.Email,
                    PhoneNumber = user.PhoneNumber,
                    IdentityCard = user.IdentityCard,
                    Gender = user.Gender,
                    BirthDate = user.BirthDate,
                    Address = user.Address,
                    Position = staff.Position,
                    Salary = staff.Salary,
                    ProvinceName = districtName,
                    DistrictName = provinceName,
                    WardName = wardName,
                };
                return Task.FromResult(staffDto); ;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi User thành UserDTO: ", ex);
            }
        }
    }
}
