using ChatApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ChatAppContext _context;

        public LoginController(ChatAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest userParam)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.Email == userParam.Email && u.Password == userParam.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Email or password is incorrect" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SJFDGHDUIRGGD876GZDF8GUJDF98GG6DF9GDG98G7D9GJUIZ0U89J0JZS42FS4FSF");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name)

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Username = user.Name,
                Token = tokenString
            });
        }
    }
}
