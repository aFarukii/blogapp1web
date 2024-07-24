using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SettingsViewModel : LayoutViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(30)]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9_.±]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "please enter valid email.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "max 20 char allowed")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public IFormFile ProfilePicture { get; set; }
    }
}
