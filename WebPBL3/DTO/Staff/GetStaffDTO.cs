using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebPBL3.DTO.Staff
{
    public class GetStaffDTO
    {
        [Key]

        [Display(Name = "Mã nhân viên")]
        [StringLength(maximumLength: 10)]
        public string StaffID { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Bạn phải nhập họ và tên")]
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(maximumLength: 15)]
        [Required(ErrorMessage = "Bạn phải nhập số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Số CCCD")]
        [StringLength(maximumLength: 12, MinimumLength = 9, ErrorMessage = "CCCD phải tối thiểu 9 và tối đa 12 chữ số.")]
        public string IdentityCard { get; set; }

        [Display(Name = "Giới tính")]
        public bool? Gender { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(maximumLength: 200, ErrorMessage = "Địa chỉ quá dài")]
        public string Address { get; set; }

        [Display(Name = "Ảnh")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Chức vụ chưa có")]
        [Display(Name = "Chức vụ")]
        [StringLength(maximumLength: 50)]
        public string Position { get; set; }

        [Display(Name = "Lương")]
        public int Salary { get; set; }
        public int WardID { get; set; }

    }
}
