using System.ComponentModel.DataAnnotations;
namespace WebPBL3.DTO
{
    public class OrderDTO
    {
        [Display(Name = "Họ và tên ")]
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

        [Display(Name = "Địa chỉ")]
        [StringLength(maximumLength: 200, ErrorMessage = "Địa chỉ quá dài")]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Ngày tạo đơn hàng")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Tổng thanh toán")]
        public double Totalprice { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public string Status { get; set; }

        [Display(Name = "Email")]
        [StringLength(maximumLength: 50)]
        [Required]
        public string Email { get; set; }
        public List<Items> items { get;set; }
        public OrderDTO()
        {
            items = new List<Items>();
        }
    }
}
