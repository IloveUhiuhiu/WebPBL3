using System.ComponentModel.DataAnnotations;

namespace WebPBL3.DTO
{
    public class FeedbackDTO
    {
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên không thể trống")]
        public string FullName { get; set; }
        [Display(Name ="Email")]
        [Required(ErrorMessage = "Email không thể trống")]
        public string Email { get; set; }
        [Display(Name = "Tiêu đề")]


        [Required(ErrorMessage = "Tiêu đề không thể trống")]
        public string Title { get; set; }


        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Nội dung không thể trống")]
        public string Content { get; set; }
        [Display(Name = "Đánh giá")]
        [Required(ErrorMessage = "Mức độ hài lòng không thể trống")]
        public int Rating { get; set; }


    }
}
