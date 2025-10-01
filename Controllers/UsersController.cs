using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Dapper;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IDbConnection _db;
    public UsersController(IDbConnection db) => _db = db;

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(sub)) return Unauthorized();

        if (!int.TryParse(sub, out var userId)) return Unauthorized();

        const string sql = "SELECT TOP 1 * FROM Users WHERE Id=@Id";
        var u = await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = userId });

        if (u is null) return NotFound();

        return Ok(new
        {
            id = u.Id,
            u.Email,
            userName = u.UserName,
            u.FirstName,
            u.LastName,
            u.RoleId
        });
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] User u)
    {
        if (u == null) return BadRequest("Invalid user data");

        const string sql = @"
INSERT INTO Users(Email,UserName,FirstName,LastName,PhoneNumber,PasswordHash,RoleId)
VALUES (@Email,@UserName,@FirstName,@LastName,@PhoneNumber,@PasswordHash,@RoleId);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var id = await _db.ExecuteScalarAsync<int>(sql, u);

        return Ok(new { UserId = id });
    }
}
