using WebApplication1.Entities;

namespace WebApplication1.Models
{
    public class TopPostsViewModel : LayoutViewModel
    {
        public IEnumerable<Post> Posts { get; set; }

    }
}
