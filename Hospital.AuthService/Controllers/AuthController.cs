using Hospital.AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthRepository repository, IConfiguration config, ILogger<AuthController> logger)
    {
        _repository = repository;
        _config = config;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _repository.ValidateUserAsync(request.Username, request.Password);
        if (user == null)
        {
            _logger.LogWarning("Login failed for username: {Username}", request.Username);
            return Unauthorized(new { message = "Invalid credentials" });
        }
        var token = GenerateJwtToken(user);
        _logger.LogInformation("User {Username} logged in successfully", user.Username);
        return Ok(new { token, role = user.Role, expires = DateTime.UtcNow.AddHours(2) });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User
        {
            Username = request.Username,
            Password = hashed,
            Role = request.Role ?? "User"
        };
        await _repository.AddUserAsync(user);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("User {Username} registered with role {Role}", user.Username, user.Role);
        return Ok(new { message = "User registered successfully", userId = user.Id });
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(2),
            claims: claims,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password, string? Role);
