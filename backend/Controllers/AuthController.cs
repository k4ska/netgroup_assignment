using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration config) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var adminEmail = config["Admin:Email"];
        var adminPassword = config["Admin:Password"];

        if (req.Email != adminEmail || req.Password != adminPassword)
            return Unauthorized(new { message = "Invalid credentials" });

        var secret = config["Jwt:Secret"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: [new Claim(ClaimTypes.Role, "Admin")],
            expires: DateTime.UtcNow.AddHours(double.Parse(config["Jwt:ExpiresInHours"]!)),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
