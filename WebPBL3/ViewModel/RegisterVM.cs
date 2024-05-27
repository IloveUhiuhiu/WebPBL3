﻿using System.ComponentModel.DataAnnotations;

namespace WebPBL3.ViewModel
{
    public class RegisterVM
    {
        [Display(Name ="Email")]
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ email")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu nhập lại")]
        public string RetypePassword { get; set; }

    }
}
