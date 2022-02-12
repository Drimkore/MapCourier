using System.ComponentModel.DataAnnotations;


namespace MapCourier.Models
{
    public class Delivery
    {
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int? id { get; set; }

        

    }
}