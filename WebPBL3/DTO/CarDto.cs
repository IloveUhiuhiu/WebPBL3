using System.ComponentModel.DataAnnotations;

namespace WebPBL3.Models
{
    public class CarDto
    {
        [Required]
        public string CarID { get; set; }
        [Required]
        public string CarName { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        public int Seat { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Dimension { get; set; }
        [Required]
        public double Capacity { get; set; }
        [Required]
        public float Topspeed { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Photo { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Engine { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FuelConsumption { get; set; }
        
        [Required]
        public string MakeName { get; set; }
    }
}
