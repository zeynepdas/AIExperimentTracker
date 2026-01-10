using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Net9RestApi.Data;
using Net9RestApi.DTOs;
using Net9RestApi.DTOs.Auth; // DTO'yu buradan tanıyacak


namespace Net9RestApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == dto.Username && !u.IsDeleted);

            if (user == null)
            {
                return Unauthorized(
                    ApiResponse<string>.Fail("Invalid username or password")
                );
            }

            // In a real application, use a secure password hashing mechanism
            if (user.PasswordHash != dto.Password)
            {
                return Unauthorized(
                    ApiResponse<string>.Fail("Invalid username or password")
                );
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:ExpireMinutes"]!)
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(
                ApiResponse<AuthResponseDto>.SuccessResponse(
                    new AuthResponseDto
                    {
                        Token = tokenString,
                        Expiration = expires
                    },
                    "Login successful"
                )
            );
        }
    }
}
