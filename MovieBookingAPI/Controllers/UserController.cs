using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MovieBookingAPI.Data;
using MovieBookingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MovieBookingAPI.DTOs;

namespace MovieBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // REGISTER
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username || u.Email == dto.Email))
                return BadRequest("Username or Email already exists.");

            var userRole = _context.Roles.Single(r => r.RoleName == "User");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = userRole.RoleId,
                Role = userRole
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Registration successful!");
        }

        // LOGIN
        [HttpPost("login")]

        public IActionResult Login([FromBody] LoginUserDto dto)
        {
            var user = _context.Users
            .Include(u => u.Role)         // Include role
            .SingleOrDefault(u => u.Username == dto.Username);

            if (user == null)
                return Unauthorized("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid password");

            // Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("id", user.UserId.ToString())
            };

            // JWT key
            var secret = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key missing");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = user.Role.RoleName,
                Username = user.Username
            });
        }

        // ✅ Get profile by username

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound();

            var profile = new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return Ok(profile);
        }



        // UPDATE PROFILE
        [Authorize]
        [HttpPut("update-profile")]

        public async Task<IActionResult> UpdateProfile(UpdateUserDto dto)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userId));
            if (user == null) return NotFound();

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }



    }
}
