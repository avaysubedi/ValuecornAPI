using Dapper;
using System.Data;

namespace ValuecornAPI.Data
{
    public interface ISqlService
    {
        Task<T> QuerySingleAsync<T>(string sp, object parameters);
        Task<int> ExecuteAsync(string sp, object parameters);
    }

    public class SqlService : ISqlService
    {
        private readonly IDbConnection _db;
        public SqlService(IDbConnection db) => _db = db;

        public async Task<T> QuerySingleAsync<T>(string sp, object parameters) =>
            await _db.QueryFirstOrDefaultAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure);

        public async Task<int> ExecuteAsync(string sp, object parameters) =>
            await _db.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure);
    }

}
