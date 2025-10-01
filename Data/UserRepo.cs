using System.Data;
using Dapper;
using ValuecornAPI.Models;

namespace ValuecornAPI.Data
{
    public interface IUserRepository
    {
        Task<User?> GetByEmail(string email);
        Task<User?> GetByUsername(string username);
        Task<int> Insert(User u);
        Task<User?> GetById(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public Task<User?> GetByEmail(string email)
        {
            const string sql = "SELECT TOP 1 * FROM Users WHERE Email=@Email";
            return _db.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public Task<User?> GetByUsername(string username)
        {
            const string sql = "SELECT TOP 1 * FROM Users WHERE UserName=@UserName";
            return _db.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });
        }

        public Task<User?> GetById(int id)
        {
            const string sql = "SELECT TOP 1 * FROM Users WHERE Id=@Id";
            return _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public Task<int> Insert(User u)
        {
            const string sql = @"
INSERT INTO Users(Email,UserName,FirstName,LastName,PhoneNumber,PasswordHash,RoleId)
VALUES (@Email,@UserName,@FirstName,@LastName,@PhoneNumber,@PasswordHash,@RoleId);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return _db.ExecuteScalarAsync<int>(sql, u);
        }
    }
}
