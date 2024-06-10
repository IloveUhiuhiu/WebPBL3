using WebPBL3.Models;
using WebPBL3.DTO;
namespace WebPBL3.Services
{
    public interface IStaffService
    {
        Task<List<StaffDTO>> GetAllStaffs();
        Task<IEnumerable<StaffDTO>> GetStaffsBySearch(string searchTerm, string searchField);
        Task<Staff> GetStaffById(string id);
        Task AddStaff(Staff staff);
        Task EditStaff(Staff staff);
        Task DeleteStaff(Staff staff);

        Staff ConvertToStaff(StaffDTO StaffDTO);
        User ConvertToUser(StaffDTO StaffDTO, string? photo);
        Account ConvertToAccount(StaffDTO StaffDTO, int roleId, string hashedPassword);

        Task<StaffDTO> ConvertToStaffDTO(Staff staff,User user, Account account);
    }
}
