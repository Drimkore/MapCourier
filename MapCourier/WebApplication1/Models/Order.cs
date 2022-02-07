using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Order
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int id { get; set; }
        //[Display(Name = "Номер заказа")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Необходим номер заказа")]
        //public int orderNumder { get; set; }
        //public string orderName { get; set; }
        [Display(Name = "Адрес")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим адрес")]
        public string address { get; set; }
        [Display(Name = "Адрес - координата долготы")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходима координата долготы")]
        public string Longitude { get; set; }
        [Display(Name = "Адрес - координата широты")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходима координата широты")]
        public string Latitude { get; set; }
        //public DateTime timeOfMakingOrder { get; set; }
    }
}
