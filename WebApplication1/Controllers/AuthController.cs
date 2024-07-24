using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using System.Drawing;
using System.Configuration;
using System.IO;

// users ve postslairn 0 olma durumunda hata veriordu

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ClaimsPrincipal _user;
        public byte[] currentProfilePic;
        public string currenUsername;
        public string currentEmail;
        public int currenUserId;

        public AuthController(AppDbContext appDbContext, IWebHostEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            _context = appDbContext;
            _environment = environment;
            _user = contextAccessor.HttpContext.User;
            GetCurrentUserInfo();

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.email == model.email && x.password == model.password).FirstOrDefault();
                if (user != null)
                {
                    //succes create cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.email),
                        new Claim(ClaimTypes.Name, user.username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())



                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    //TempData["username"] = user.username;
                    //TempData["email"] = user.email;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is not correct.");
                }
            }
            return View(model);

        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        public IActionResult SecurePage()
        {   
            ViewBag.Name = HttpContext.User.Identity.Name;

            return View();
        }
            public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid) {

                User user = new User();
                user.username = model.username;
                user.password = model.password; 
                user.email = model.email;

                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"Mail: {user.email} Username: {user.username}  Please login.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Please enter unique email. Ex={ex}");
                    return View(model);
                }
                return View();
            }
            return View(model);
        }
        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Settings()
        {
            var user = _context.Users.Where(x => x.Id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var viewModel = new SettingsViewModel
            {
                username = user.username,
                email = user.email,
                password = user.password,
                currentUserPic = currentProfilePic,
                currentEmail = currentEmail,
                currentUsername = currenUsername
            };

            return View(viewModel);
        }

        [HttpPost]
        public async  Task<IActionResult> Settings(SettingsViewModel model)
        {
            var user = _context.Users.Where(x => x.Id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
            string message = "";
                if (!string.IsNullOrEmpty(model.username))
                {
                    user.username = model.username;
                    message += $"New username: {model.username} ";
                }
                if (!string.IsNullOrEmpty(model.password))
                {
                    user.password = model.password;
                    message += $"New password: {model.password} ";

                }
                if (!string.IsNullOrEmpty(model.email))
                {
                    user.email = model.email;
                    message += $"New email: {model.email} ";

                }
                if (model.ProfilePicture != null)
                {
                    if (IsValidImage(model.ProfilePicture)) {
                        var fileName = GenerateUniqueFileName(model.ProfilePicture.FileName);
                        var profilePic = new ProfilePic();
                        using (var memoryStream = new MemoryStream())
                        {
                            await model.ProfilePicture.OpenReadStream().CopyToAsync(memoryStream);
                            profilePic.Content = memoryStream.ToArray();
                        }
                        profilePic.FileName = fileName;
                    if (user.ProfilePicId != null)
                    {
                        var existingProfilePic = await _context.ProfilePic.FindAsync(user.ProfilePicId);

                        if (existingProfilePic != null)
                        {
                            _context.ProfilePic.Remove(existingProfilePic);
                            _context.SaveChanges();

                        }
                    }
                    _context.ProfilePic.Add(profilePic);
                        _context.SaveChanges();
                        user.ProfilePicId = profilePic.Id;
                }

            }
                _context.SaveChanges();
                ViewBag.Message = message;
                var viewModel = new SettingsViewModel
                {
                    username = user.username,
                    email = user.email,
                    password = user.password,
                };
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var claims = new List<Claim>
              {
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
              };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return View(viewModel);
            
        }
            private bool IsValidImage(IFormFile file)
            {
                if (file == null || file.Length == 0 || file.Length >= 2e+6)
                {
                    return false;
                }

                var allowedTypes = new List<string> { "image/jpeg", "image/png", "image/gif" };
                return allowedTypes.Contains(file.ContentType);
            }
        private string GenerateUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            var randomString = Guid.NewGuid().ToString("N"); 

           
            return $"{randomString}{extension}";
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

