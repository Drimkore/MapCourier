using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Storage
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int id { get; set; }
        [Display(Name = "Название склада")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо название склада")]
        public string storageName { get; set; }
        [Display(Name = "Адрес склада")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим адрес склада")]
        public string storageAddress { get; set; }
        [Display(Name = "Адрес склада - координата долготы")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходима координата долготы склада")]
        public string coordinateLongitude { get; set; }
        [Display(Name = "Адрес склада - координата широты")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходима координата широты склада")]
        public string coordinateLatitude { get; set; }
    }
}
