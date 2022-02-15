using System.ComponentModel.DataAnnotations;


namespace MapCourier.Models
{
    public class Delivery
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int DeliveryID { get; set; }
        public int? StorageID { get; set; }
        public Storage Storage { get; set; } 
        public int? OrderID { get; set; }
        public Order Order { get; set; }
        public string? UserID { get; set; }     

    }
}