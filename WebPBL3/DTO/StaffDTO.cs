﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebPBL3.DTO
{
    using System.Web;
    public class StaffDTO : UserDTO
    {
        public string? StaffID { get; set; }
        [Display(Name = "Chức vụ")]
        [Required(ErrorMessage = "Vị trí nhân viên là bắt buộc.")]
        public string Position { get; set; } = string.Empty;

        [Display(Name = "Lương")]
        [Required(ErrorMessage = "Lương là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Lương phải là số dương.")]
        public int Salary { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
