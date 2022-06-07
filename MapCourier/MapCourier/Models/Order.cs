using System.ComponentModel.DataAnnotations;

namespace MapCourier.Models
{
    public class Order
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int OrderID { get; set; }
        [Display(Name = "Адрес")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим адрес")]
        public string address { get; set; }
        [Display(Name = "Адрес - координата долготы")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходима координата долготы")]
        public string Longitude { get; set; }
        [Display(Name = "Адрес - координата широты")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходима координата широты")]
        public string Latitude { get; set; }
        [Display(Name = "Статус заказа")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите, доставляется ли заказ")]
        public string status { get; set; }        //"waiting"/"busy"/"finished"
        [Display(Name = "Начало назначенного времени доставки")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует начало временных рамок доставки")]
        public DateTime TimeFrameBeginning { get; set; }
        [Display(Name = "Окончание назначенного времени доставки")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует конец временных рамок доставки")]
        public DateTime TimeFrameEnding { get; set; }
    }
}
