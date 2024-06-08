using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebPBL3.DTO;
using WebPBL3.Models;

namespace WebPBL3.Services
{
	public class UserService : IUserService
	{
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment _environment;
        public UserService(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        public async Task AddUser(User user)
		{
			try
			{
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
			} 
			catch (Exception ex)
			{
                throw new Exception("Lỗi xuất hiện khi thêm người dùng: ", ex);
            }

		}
        public async Task DeleteUser(User user)
        {

            try
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xảy ra khi xóa người dùng: ", ex);
            }
        }

        public async Task EditUser(User user)
        {
            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xuất hiện khi cập nhật người dùng: ", ex);
            }
        }
        public async Task<User> GetUserById(string id)
        {
          
            try
            {
                User? user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == id);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xảy ra khi truy vấn người dùng: ", ex);
            }


        }

        public User ConvertToUser(UserDTO UserDTO)
        {   
            try
            {
                User user = new User
                {
                    UserID = UserDTO.UserID,
                    FullName = UserDTO.FullName,
                    PhoneNumber = UserDTO.PhoneNumber,
                    IdentityCard = UserDTO.IdentityCard,
                    Gender = UserDTO.Gender,
                    Address = UserDTO.Address,
                    BirthDate = UserDTO.BirthDate,
                    Photo = UserDTO.Photo,
                    WardID = UserDTO.WardID > 0 ? UserDTO.WardID : null,
                    AccountID = UserDTO.AccountID,
                };
                return user;
            } catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi UserDTO thành User: ", ex);
            }
            


        }

		public async Task<UserDTO> ConvertToUserDTO(User user)
		{
            
            Account? account = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountID == user.AccountID);
            if (account == null)
            {
                throw new InvalidOperationException($"Account với ID {user.AccountID} không được tìm thấy.");
            }
            try
            {
                UserDTO UserDTO = new UserDTO
                {
                    UserID = user.AccountID,
                    FullName = user.FullName,
                    Email = account.Email,
                    PhoneNumber = user.PhoneNumber,
                    IdentityCard = user.IdentityCard,
                    Gender = user.Gender,
                    BirthDate = user.BirthDate,
                    Address = user.Address,
                    WardID = user.WardID,
                    Photo = user.Photo,
                };
                return UserDTO;
            } catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi User thành UserDTO: ", ex);
            }
            
        }

		

        public async Task<IEnumerable<UserDTO>> GetAllUsers(string searchtxt, int fieldsearch, int page)
        {
            List<UserDTO> users = await _db.Users
                .Include(a => a.Account)
                .Include(w => w.Ward)
                .ThenInclude(d => d.District)
                .Where(u => u.Account.RoleID == 3 && (searchtxt.IsNullOrEmpty()
                || (fieldsearch == 1 && u.FullName.Contains(searchtxt))
                || (fieldsearch == 2 && u.PhoneNumber.Contains(searchtxt))
                || (fieldsearch == 3 && u.Account.Email.Contains(searchtxt))
                || (fieldsearch == 4 && u.IdentityCard.Contains(searchtxt))))
                .Select(u => new UserDTO
                {
                    AccountID = u.AccountID,
                    Email = u.Account.Email,
                    Password = u.Account.Password,
                    Status = u.Account.Status,

                    RoleID = 3,
                    UserID = u.UserID,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    IdentityCard = u.IdentityCard,

                    Gender = u.Gender,
                    BirthDate = u.BirthDate,
                    Address = u.Address,
                    Photo = u.Photo,
                    WardID = u.WardID,
                    WardName = u.Ward.WardName,
                    DistrictName = u.Ward.District.DistrictName,
                    ProvinceName = u.Ward.District.Province.ProvinceName,

                }).ToListAsync();
                return users;
        }

        
	}
}
