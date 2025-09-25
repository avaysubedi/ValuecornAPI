using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ValuecornAPI.Models;

namespace ValuecornAPI.Data;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUsername(string username);
    Task<int> Insert(User u);
    Task<User?> GetById(int id);
}

public class UserRepository : IUserRepository
{
    private readonly IConfiguration _cfg;
    public UserRepository(IConfiguration cfg) => _cfg = cfg;
    private IDbConnection Conn => new SqlConnection(_cfg.GetConnectionString("DefaultConnection"));

    public async Task<User?> GetByEmail(string email)
    {
        const string sql = "SELECT TOP 1 * FROM Users WHERE Email=@Email";
        using var c = Conn; return await c.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByUsername(string username)
    {
        const string sql = "SELECT TOP 1 * FROM Users WHERE UserName=@UserName";
        using var c = Conn; return await c.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });
    }

    public async Task<User?> GetById(int id)
    {
        const string sql = "SELECT TOP 1 * FROM Users WHERE Id=@Id";
        using var c = Conn; return await c.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<int> Insert(User u)
    {
        const string sql = @"
INSERT INTO Users(Email,UserName,FirstName,LastName,PhoneNumber,PasswordHash,RoleId)
VALUES (@Email,@UserName,@FirstName,@LastName,@PhoneNumber,@PasswordHash,@RoleId);
SELECT CAST(SCOPE_IDENTITY() AS INT);";
        using var c = Conn; return await c.ExecuteScalarAsync<int>(sql, u);
    }
}
