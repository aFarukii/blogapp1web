using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PostViewModel
    {

        [Required(ErrorMessage = "Writer user ID is required")]

        [ForeignKey("Id")]
        public int PostUserId { get; set; }


        [Required(ErrorMessage = "Content is required")]
        public string content { get; set; }
    }
}
