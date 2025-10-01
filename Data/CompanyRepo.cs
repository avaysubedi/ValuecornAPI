using Dapper;
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
        private readonly IDbConnection _db;

        public CompanyRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> UpsertCompanyAsync(CompanyDto company, int userId)
        {
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

            await _db.ExecuteAsync(
                "sp_UpsertCompanyCore",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@CompanyId");
        }
    }
}
