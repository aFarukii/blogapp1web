using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Entities;
using WebApplication1.Models;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Drawing;



namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ClaimsPrincipal _user;
        public byte[] currentProfilePic;
        public string currenUsername;
        public string currentEmail;
        public int currenUserId;
        public HomeController(AppDbContext appDbContext, IWebHostEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            _context = appDbContext;
            _environment = environment;
            _user = contextAccessor.HttpContext.User;
            GetCurrentUserInfo();

        }
        public IActionResult Index()
        {
            return View(GetIndex(currentProfilePic));


        }

        public IActionResult About()
        {
            return View(LayoutViewModel());
        }
        public IActionResult Status()
        {

            return View(LayoutViewModel());

        }
        public IActionResult Blog()
        {
            return View(GetPosts());
        }
        [HttpPost]
        public IActionResult Blog(PostViewModel model)
        {
            return View(UploadPosts(model));
        }

        public IndexViewModel GetIndex(byte[] currentProfilePic)
        {
            var lastUsers = _context.Users.OrderByDescending(u => u.userDate).Include(u => u.ProfilePic).Take(5).ToList();
            var posts = _context.Posts.Include(u => u.User).Include(u => u.User.ProfilePic).OrderByDescending(p => p.CreationDate).Take(10).ToList();
            var viewModel = new IndexViewModel
            {
                TopPosts = new TopPostsViewModel { Posts = posts },
                LatestUsers = new TopUsersViewModel { Users = lastUsers },
                currentUserPic = currentProfilePic,
                currentEmail = currentEmail,
                currentUsername = currenUsername

            };
            return viewModel;
        }
        public LayoutViewModel LayoutViewModel()
        {

            var viewModel = new LayoutViewModel

            {
                currentUserPic = currentProfilePic,
                currentEmail = currentEmail,
                currentUsername = currenUsername

            };

            return viewModel;

        }
        public TopPostsViewModel GetPosts()
        {
            var posts = _context.Posts.Include(u => u.User).Include(u => u.User.ProfilePic).OrderByDescending(p => p.CreationDate).Take(10).ToList();

            var viewModel = new TopPostsViewModel

            {
                Posts = posts,
                currentUserPic = currentProfilePic,
                currentEmail = currentEmail,
                currentUsername = currenUsername

            };
            return viewModel;
        }

        public Object UploadPosts(PostViewModel model)
        {
            if (ModelState.IsValid)
            {

                Post post = new Post();
                post.content = model.content;
                post.PostUserId = currenUserId;

                try
                {
                    _context.Posts.Add(post);
                    _context.SaveChanges();
                    ModelState.Clear();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Invalid post type. EX= {ex}");
                    return model;
                }

                return GetPosts();
            }
            return GetPosts();


        }

        public void GetCurrentUserInfo()
        {
            if (_user.Identity.IsAuthenticated)
            {
                currenUsername = _user.FindFirstValue(ClaimTypes.Name);
                currentEmail = _user.FindFirstValue(ClaimTypes.Email);
                currenUserId = int.Parse(_user.FindFirstValue(ClaimTypes.NameIdentifier));
                currentProfilePic = _context.Users
                .Include(p => p.ProfilePic)
                .Where(u => u.Id == int.Parse(_user.FindFirstValue(ClaimTypes.NameIdentifier)))
                .Select(u => u.ProfilePic.Content)
                .FirstOrDefault();
            }
        }
    }
}
