﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebPBL3.DTO
{
    using System.Web;
    public partial class StaffDTO
    {
        public string? StaffID { get; set; }
        public string? UserID { get; set; }
        public string? AccountID { get; set; }

        [Required(ErrorMessage = "Họ và Tên là bắt buộc.")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^(0[0-9]{9,10})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "CCCD là bắt buộc.")]
        [RegularExpression(@"^(0[0-9]{9,10})$", ErrorMessage = "CCCD không hợp lệ.")]
        public string? IdentityCard { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Vị trí nhân viên là bắt buộc.")]
        public string Position { get; set; }
        [Required(ErrorMessage = "Lương là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Lương phải là số dương.")]
        public int Salary { get; set; }
        public int? WardID { get; set; }
        public string? WardName { get; set; }
        public string? DistrictName { get; set; }
        public string? ProvinceName { get; set; }
        [Required(ErrorMessage = "Email là bắt buộc.")]
        public string Email { get; set; }
        public string? Password { get; set; }
        public int? RoleID { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
