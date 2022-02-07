using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходим id")]
        public int id { get; set; }
        [Display(Name = "Login")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "")]
        public string Login { get; set; }
        [Display(Name = "password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "")]
        public string Password { get; set; }
    }
}
