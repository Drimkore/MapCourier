using System.ComponentModel.DataAnnotations;


namespace MapCourier.Models
{
    public class DeliveryLog
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int DeliveryLogID { get; set; }
        public int? StorageID { get; set; }
        public Storage Storage { get; set; } 
        public int? OrderID { get; set; }
        public Order Order { get; set; }
        public string? UserID { get; set; }   
    }
}