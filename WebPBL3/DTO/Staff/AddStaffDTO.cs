using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebPBL3.DTO.Staff
{
    using System.Web;
    public partial class AddStaffDTO
    {
        [Key]
        public string? StaffID { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityCard { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string Position { get; set; }
        public int Salary { get; set; }
        public string? WardName { get; set; }
        public string? DistrictName { get; set; }
        public string? ProvinceName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
