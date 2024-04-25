using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

using RWA_MVC_JAVNI.Data;
using RWA_MVC_JAVNI.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace RWA_MVC_JAVNI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }public IActionResult Account()
        {
            var viewModel = new User();


            viewModel.Id = int.Parse(HttpContext.Session.GetString("Id"));
            viewModel.Username = HttpContext.Session.GetString("Username");
            viewModel.FirstName = HttpContext.Session.GetString("FirstName");
            viewModel.LastName = HttpContext.Session.GetString("LastName");
            viewModel.Email = HttpContext.Session.GetString("Email");
            viewModel.Phone = HttpContext.Session.GetString("Phone");


            return View(viewModel);
        }
        private readonly AppDbContext _context;
        //private readonly IJwtTokenService _jwtTokenService;
        public UserController(AppDbContext context/*, IJwtTokenService jwtTokenService*/)
        {
            _context = context;
            //_jwtTokenService = jwtTokenService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            var countries = _context.Country.ToList();
            var model = new User
            {
                Countries = countries
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(User addUserRequest)
        {
            if (!IsValidEmail(addUserRequest.Email))
            {
                ModelState.AddModelError("Email", "Invalid email address.");
           
                return View(addUserRequest);
            }
            string salt = Encryption.CreateSalt(8);
            string password = addUserRequest.PwdHash;
            var user = new User()
            {
                Username = addUserRequest.Username,
                FirstName = addUserRequest.FirstName,
                LastName = addUserRequest.LastName,
                Email = addUserRequest.Email,
                Phone = addUserRequest.Phone,
                CreatedAt = DateTime.Now,
                PwdSalt = salt,
                PwdHash = Encryption.GenerateHash(password, salt),
                CountryOfResidenceId = addUserRequest.CountryOfResidenceId
            };

          await  _context.User.AddAsync(user);
            await _context.SaveChangesAsync();


            //var token = _jwtTokenService.GenerateToken(user);

            //HttpContext.Response.Cookies.Append("AccessToken", token, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.None // Adjust based on your requirements
            //});
            var session = HttpContext.Session;
            session.SetString("Id",user.Id.ToString());
            session.SetString("Username",user.Username);
            session.SetString("FirstName", user.FirstName);
            session.SetString("LastName", user.LastName);
            session.SetString("Email", user.Email);
            session.SetString("Phone", user.Phone);
        

            return RedirectToAction("Index","Video");
            
     
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Login(User loginUserRequest)
        {
          
            string emailToFind = loginUserRequest.Email;
            var user = _context.User.FirstOrDefault(u => u.Email == emailToFind);




            if (user != null )
            {
                if (Encryption.ValidatePassword(loginUserRequest.PwdHash, user.PwdSalt, user.PwdHash))
                {
                    //var token = _jwtTokenService.GenerateToken(user);

                    //HttpContext.Response.Cookies.Append("AccessToken", token, new CookieOptions
                    //{
                    //    HttpOnly = true,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.None
                    //});
                    // Password is correct
                    var session = HttpContext.Session;
                    session.SetString("Id", user.Id.ToString());
                    session.SetString("Username", user.Username);
                    session.SetString("FirstName", user.FirstName);
                    session.SetString("LastName", user.LastName);
                    session.SetString("Email", user.Email);
                    session.SetString("Phone", user.Phone);
                    return RedirectToAction("Index", "Video");
                }
              
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginUserRequest);
            }
            else
            {
             
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginUserRequest);
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
          
            HttpContext.Session.Clear();
            //Response.Cookies.Delete("AccessToken");
          
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordModel)
        {
           
                var userId = int.Parse(HttpContext.Session.GetString("Id"));
            var user = _context.User.FirstOrDefault(u => u.Id == userId);


            if (user != null && Encryption.ValidatePassword(changePasswordModel.CurrentPassword, user.PwdSalt, user.PwdHash))
                {
                    string newSalt = Encryption.CreateSalt(8);
                    string newPassword = changePasswordModel.NewPassword;
                    user.PwdSalt = newSalt;
                    user.PwdHash = Encryption.GenerateHash(newPassword, newSalt);

                    _context.User.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Password changed successfully!";
                    return RedirectToAction("Account");
                }
                else
                {
                    ModelState.AddModelError(nameof(ChangePasswordViewModel.CurrentPassword), "Invalid current password");
                }
            return View(changePasswordModel);
        }
     
        
    }
}
