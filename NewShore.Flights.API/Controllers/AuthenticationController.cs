using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewShore.Flights.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;

        public AuthenticationController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            string strToken = GenerateToken();

            return Ok(new { token = strToken });
        }

        private string GenerateToken()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                                        claims: claims,
                                        expires: DateTime.Now.AddMinutes(60),
                                        signingCredentials: credentials
                                    );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
