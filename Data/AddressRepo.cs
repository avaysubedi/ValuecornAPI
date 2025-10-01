using Dapper;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Data
{
    public interface IAddressRepository
    {
        Task<int> UpsertAddress(CompanyAddress address);
    }

    public class AddressRepository : IAddressRepository
    {
        private readonly IDbConnection _db;

        public AddressRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> UpsertAddress(CompanyAddress address)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AddressId", address.AddressId);
            parameters.Add("@CompanyId", address.CompanyId);
            parameters.Add("@AddressType", address.AddressType);
            parameters.Add("@AddressLine", address.AddressLine);
            parameters.Add("@Street", address.Street);
            parameters.Add("@City", address.City);
            parameters.Add("@State", address.State);
            parameters.Add("@PostalCode", address.PostalCode);
            parameters.Add("@POBox", address.POBox);
            parameters.Add("@CountryCode", address.CountryCode);
            parameters.Add("@UserId", address.UserId);

            return await _db.ExecuteScalarAsync<int>(
                "sp_UpsertCompanyAddress",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
