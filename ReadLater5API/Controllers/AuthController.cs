using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReadLater5API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReadLater5API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] Credential credential)
        {
            var user = await _userManager.FindByEmailAsync(credential.Email);

            if (user == null)
                return Unauthorized("User doesn't exist.");

            var verifyPassword = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, credential.Password);

            if (verifyPassword == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid password.");

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(30);

            return Ok(new
            {
                access_token = CreateToken(claims, expiresAt),
                expires_at = expiresAt
            });
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));

            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                ));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
