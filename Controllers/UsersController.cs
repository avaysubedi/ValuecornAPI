using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ValuecornAPI.Data;

namespace ValuecornAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;
    public UsersController(IUserRepository users) => _users = users;

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub"); // depending on handler
        if (string.IsNullOrEmpty(sub)) return Unauthorized();

        if (!int.TryParse(sub, out var userId)) return Unauthorized();

        var u = await _users.GetById(userId);
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
}
