using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoizeController : ControllerBase
    {
        private readonly libraryContext _context;

        public AuthoizeController(libraryContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(string username, string password)
        {
            var userInfo = await _context.Users.SingleOrDefaultAsync(user => user.Username == username && user.Password == password);
            if (userInfo != null)
            {
                var claims = new Claim[]
                {
                    new Claim("Id", userInfo.UserID.ToString()),
                    new Claim("Username", userInfo.Username),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C07F25AF-4EAF-649E-E086-AB21292E7095"));
                var token = new JwtSecurityToken(
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(jwtToken);
            }
            else
            {
                return Problem("用户名或密码错误！");
            }
        }
    }
}
