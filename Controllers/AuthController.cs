using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using ValuecornAPI.Models;
using ValuecornAPI.Data;
using ValuecornAPI.Auth;

namespace ValuecornAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly IConfiguration _cfg;

    public AuthController(IUserRepository users, IConfiguration cfg)
    {
        _users = users;
        _cfg = cfg;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        // Email/Username uniqueness checks
        if (await _users.GetByEmail(req.Email) is not null)
            return BadRequest(new { error = "Email already registered" });

        if (await _users.GetByUsername(req.Username) is not null)
            return BadRequest(new { error = "Username already taken" });

        var hashed = BCrypt.Net.BCrypt.HashPassword(req.Password);

        var user = new User
        {
            Email = req.Email,
            UserName = req.Username,
            FirstName = req.FirstName,
            LastName = req.LastName,
            PhoneNumber = req.PhoneNumber,
            PasswordHash = hashed,
            RoleId = 1
        };

        var newId = await _users.Insert(user);

        return StatusCode(201, new { message = "User registered successfully", userId = newId });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await _users.GetByEmail(req.Email);
        if (user is null) return Unauthorized(new { error = "Invalid credentials" });

        var ok = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);
        if (!ok) return Unauthorized(new { error = "Invalid credentials" });

        var token = JwtHelper.CreateToken(user.Id, user.Email, user.RoleId, _cfg);
        return Ok(new AuthResponse { Message = "Login successful", Token = token });
    }
}
