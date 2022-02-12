using System.ComponentModel.DataAnnotations;


namespace MapCourier.Models
{
    public class DeliveryLog
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int id { get; set; }
    }
}