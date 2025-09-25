using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ValuecornAPI.Auth;

public static class JwtHelper
{
    public static string CreateToken(int userId, string email, int roleId, IConfiguration cfg)
    {
        var key = cfg["Jwt:Key"]!;
        var issuer = cfg["Jwt:Issuer"];
        var audience = cfg["Jwt:Audience"];
        var days = int.TryParse(cfg["Jwt:DaysValid"], out var d) ? d : 7;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, roleId.ToString()) // role as string (e.g., "1")
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(days),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
