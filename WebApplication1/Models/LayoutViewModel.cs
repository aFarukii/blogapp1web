using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LayoutViewModel
    {
        public string? currentUsername { get; set; }

        public string? currentEmail { get; set; }
        public byte[]? currentUserPic { get; set; }

    }
}
