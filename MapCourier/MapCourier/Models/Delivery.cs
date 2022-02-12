using System.ComponentModel.DataAnnotations;


namespace MapCourier.Models
{
    public class Delivery
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int DeliveryID { get; set; }
        public int? StorageID { get; set; }
        public int? OrderID { get; set; }
        public int? UserID { get; set; }

        

    }
}