using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities
{

    [Index(nameof(email), IsUnique =true)]
        
    public class User 
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Email is required.")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(30)]

        public string username { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime userDate { get; set; } = DateTime.Now;


        public int? ProfilePicId { get; set; } // nullable foreign key for optional profile picture

        [ForeignKey(nameof(ProfilePicId))]
        public ProfilePic ProfilePic { get; set; } // navigation property for ProfilePic
    }
}
