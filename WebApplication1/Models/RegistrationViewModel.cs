using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RegistrationViewModel : LayoutViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength =6, ErrorMessage = "max 20 char allowed" )]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(30)]
        public string username { get; set; }

        [Compare("password", ErrorMessage ="please confirm your password.")]
        [DataType(DataType.Password)]

        public string confirmPassword { get; set; }
    }
}
