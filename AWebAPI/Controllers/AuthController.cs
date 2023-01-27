using AWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetToken(string username, string password)
        {
            if (!BCrypt.Net.BCrypt.Verify(password, BCrypt.Net.BCrypt.HashPassword(password)))
            {
                return BadRequest("Kullanıcı adı veya şifre hatalı !");
            }
            return Ok(await CreateToken(new AppUser() { UserName = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) }));
        }

        private async Task<string> CreateToken(AppUser user)
        {
            string result = "";
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name,user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalVariables.Token));
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var jwt = new JwtSecurityToken(claims:claims, expires:DateTime.Now.AddDays(1),signingCredentials:cred);
            result = new JwtSecurityTokenHandler().WriteToken(jwt);
            return result;
        }
    }
}
