using System.ComponentModel.DataAnnotations;

namespace MapCourier.Models
{
    public class User
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int id { get; set; }
        [Display(Name = "Login")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Неверный Login")]
        public string Login { get; set; }
        [Display(Name = "password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Неверный пароль")]
        public string Password { get; set; }
    }
}
