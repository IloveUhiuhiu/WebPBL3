using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebPBL3.Models;

namespace WebPBL3.Models
{
    public class Feedback
    {
        public Feedback()
        {
            FeedbackID = UserID = Content = Status = string.Empty;
        }

        [Key]
        [Display(Name = "Mã phản hồi")]
        [StringLength(maximumLength: 10)]
        public string FeedbackID { get; set; }


        [Display(Name = "Ngày đăng")]
        [Required(ErrorMessage = "Ngày đăng không thể trống")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Trạng thái")]
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Trạng thái không thể trống")]
        public string Status { get; set; }

        [Display(Name = "Mức độ hài lòng")]
        [Required(ErrorMessage = "Mức độ hài lòng không thể trống")]
        public int Rating { get; set; }

        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Nội dung không thể trống")]
        public string Content { get; set; }

        public User User { get; set; }
        public string UserID { get; set; }

    }
}