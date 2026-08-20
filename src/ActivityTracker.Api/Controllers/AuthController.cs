using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ActivityTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(IAuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var result = _authService.ValidateCredentials(dto);
        if (result is null)
            return Unauthorized(new { message = "Usuario o contrase\u00f1a incorrectos." });

        var response = GenerateToken(result.Value.Username, result.Value.Role);
        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var username = User.Identity?.Name ?? "unknown";
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
        return Ok(new { username, role });
    }

    private AuthResponseDto GenerateToken(string username, string role)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiryHours = int.Parse(jwtSection["ExpiryHours"] ?? "8");
        var expiresAt = DateTime.UtcNow.AddHours(expiryHours);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return new AuthResponseDto(
            Token: new JwtSecurityTokenHandler().WriteToken(token),
            Username: username,
            Role: role,
            ExpiresAt: expiresAt);
    }
}
