using FinancePlus.Data;
using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using FinancePlus.Requests;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace FinancePlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required.");

            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // email kullanilmis mi
            var existingUser = await _userService.GetAllAsync();
            if (existingUser.Any(u => u.Email == request.Email))
                return BadRequest("Email already in use.");

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                IBAN = request.IBAN,
                Currency = request.Currency,
                UserType = (UserType) request.UserType,
                Password = request.Password 
            };

            var createdUser = await _userService.CreateAsync(user);

            var token = _tokenService.GenerateToken(createdUser);
            return Ok(new { token });
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
