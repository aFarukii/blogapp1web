using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Creation date is required")]
        public DateTime CreationDate { get; set; } = DateTime.Now;  

        [Required(ErrorMessage = "Writer user ID is required")]
        [ForeignKey("User")] 
        public int PostUserId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string content { get; set; }

        [ForeignKey("PostUserId")]  
        public User User { get; set; }
    }
}
