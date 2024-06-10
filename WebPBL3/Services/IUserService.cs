using WebPBL3.Models;
using WebPBL3.DTO;
namespace WebPBL3.Services
{
	public interface IUserService
	{
		Task<IEnumerable<UserDTO>> GetAllUsers(string searchtxt,int fieldsearch, int page);
		Task<User> GetUserById(string id);
		Task AddUser(User user);
        Task EditUser(User user);
        Task DeleteUser(User user);
		User ConvertToUser(UserDTO UserDTO);
		Task<UserDTO> ConvertToUserDTO(User user);
		Task<UserDTO> ExtractEmail(string email);
	}
}
