using System.ComponentModel.DataAnnotations;

namespace WebPBL3.ViewModel
{
    public class LoginVM
    {
        [Display(Name = "Email")]
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Bạn phải nhập email")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Bạn phải nhập password")]
        public string Password { get; set; }
    }
}
