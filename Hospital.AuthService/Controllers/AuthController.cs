using Hospital.AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Hospital.AuthService.DTOs.AuthDtos;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthRepository repository,
        IConfiguration config,
        ILogger<AuthController> logger
        )
    {
        _repository = repository;
        _config = config;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var user = await _repository.ValidateUserAsync(dto.Username, dto.Password);
        if (user == null)
        {
            _logger.LogWarning("Login failed for username: {Username}", dto.Username);
            return Unauthorized(new 
            {
                message = "Invalid username or password." 
            });
        }
        var token = GenerateJwtToken(user);
        _logger.LogInformation("User {Username} logged in successfully", user.Username);

        return Ok(new 
        {
            token,
            role = user.Role, 
            expires = DateTime.UtcNow.AddHours(2)
        });
    }

    [AllowAnonymous]
    [HttpPost("register")]

    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        // Duplicate username guard
        if (await _repository.UsernameExistsAsync(dto.Username))
        {
            _logger.LogWarning("Registration failed — duplicate username: {Username}", dto.Username);
            return Conflict(new
            { message = $"Username '{dto.Username}' is already taken." }
            );
        }

        var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User
        {
            Username = dto.Username,
            Password = hashed,
            Role = dto.Role ?? "User"
        };

        await _repository.AddUserAsync(user);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("User {Username} registered with role {Role}", user.Username, user.Role);

        return Ok(new
        { message = "User registered successfully", 
            userId = user.Id
        });
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

