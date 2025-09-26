using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Data
{
    public interface ICompanyRepository
    {
        Task<int> UpsertCompanyAsync(CompanyDto company, int userId);
    }

    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<int> UpsertCompanyAsync(CompanyDto company, int userId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@UserId", userId);

            parameters.Add("@CompanyCode", company.CompanyCode);
            parameters.Add("@CompanyName", company.CompanyName);
            parameters.Add("@TradeName", company.TradeName);
            parameters.Add("@DateOfIncorporation", company.DateOfIncorporation);
            parameters.Add("@CountryCode", company.CountryCode);
            parameters.Add("@LegalStructure", company.LegalStructure);
            parameters.Add("@BusinessLicenseNumber", company.BusinessLicenseNumber);
            parameters.Add("@LicenseExpiryDate", company.LicenseExpiryDate);
            parameters.Add("@OfficialPhone", company.OfficialPhone);
            parameters.Add("@OfficialEmail", company.OfficialEmail);
            parameters.Add("@Website", company.Website);
            parameters.Add("@IndustryCode", company.IndustryCode);
            parameters.Add("@Currency", company.Currency);
            await connection.ExecuteAsync(
                "sp_UpsertCompanyCore",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@CompanyId");
        }
    }

}
