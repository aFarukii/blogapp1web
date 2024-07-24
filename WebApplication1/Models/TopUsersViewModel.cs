using WebApplication1.Entities;

namespace WebApplication1.Models
{
    public class TopUsersViewModel : LayoutViewModel
    {
        public IEnumerable<User> Users { get; set; }


    }
}
