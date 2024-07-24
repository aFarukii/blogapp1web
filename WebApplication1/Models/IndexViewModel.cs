namespace WebApplication1.Models
{
    public class IndexViewModel : LayoutViewModel
    {
        public TopPostsViewModel TopPosts { get; set; }
        public TopUsersViewModel LatestUsers { get; set; }
    }
}
