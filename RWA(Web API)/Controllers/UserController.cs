using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;

using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RWA_Web_API_.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Build.Framework;
using RWA_Web_API_.Data;

namespace RWA_Web_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private IConfiguration _config;
        private readonly UserContext _dbContext;

      
        public UserController(/*IConfiguration configuration,*/ UserContext dbContext)
        {
            _dbContext = dbContext;
            //_config = configuration;

        }
        //private User AuthenticateUser(User user)
        //{
        //    User _user=null;
        //    if (user.Username=="admin"&&user.Password=="1234")
        //    {
        //        _user = new User
        //        {
        //            Username = "Borna"
        //        };
        //    }
        //    return _user;               
        //}
        //private string GenerateToken(User users)
        //{
        //    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials=new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],null,
        //        expires: DateTime.Now.AddMinutes(1),
        //        signingCredentials:credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        //public IActionResult Login(User user)
        //{
        //    IActionResult response = Unauthorized();
        //    var user_ = AuthenticateUser(user);
        //    if (user_ != null)
        //    {
        //        var token = GenerateToken(user_);
        //        response = Ok(new { token = token });
        //    }
        //    return response;
        //}

        [HttpPost]
        public async Task<IActionResult> Add(User addUserRequest)
        {
            //Register User
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
                CountryOfResidenceId = addUserRequest.CountryOfResidenceId,
            };

            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user); 
        }


    }
}
