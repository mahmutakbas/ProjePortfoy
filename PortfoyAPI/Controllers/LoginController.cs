using Business.Concrete;
using Entities.DTOs.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebPortfoy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config, IUserService userService)
        {
            _userService = userService;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] Login userLogin)
        {
            if (string.IsNullOrEmpty(userLogin.UserName) && string.IsNullOrEmpty(userLogin.Password))
                return BadRequest(new { isSuccess = false, Message = "User bulunamadı" });

            var user = await _userService.Login(userLogin?.UserName!, userLogin?.Password!);
            if (user.Id != 0)
            {
                var token = GenerateToken(user);
                return Ok(new { Token = token, isSuccess = true });
            }

            return Unauthorized(new { isSuccess = false });
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username!),
                new Claim(ClaimTypes.Role,"Admin")
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
