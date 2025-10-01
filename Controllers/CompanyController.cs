using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IDbConnection _db;

        public CompanyController(IDbConnection db)
        {
            _db = db;
        }

        // POST: api/company
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid company data.");

            var userId = GetUserId();

            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", dto.CompanyId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@UserId", userId);
            parameters.Add("@CompanyCode", dto.CompanyCode);
            parameters.Add("@CompanyName", dto.CompanyName);
            parameters.Add("@TradeName", dto.TradeName);
            parameters.Add("@DateOfIncorporation", dto.DateOfIncorporation);
            parameters.Add("@CountryCode", dto.CountryCode);
            parameters.Add("@LegalStructure", dto.LegalStructure);
            parameters.Add("@BusinessLicenseNumber", dto.BusinessLicenseNumber);
            parameters.Add("@LicenseExpiryDate", dto.LicenseExpiryDate);
            parameters.Add("@OfficialPhone", dto.OfficialPhone);
            parameters.Add("@OfficialEmail", dto.OfficialEmail);
            parameters.Add("@Website", dto.Website);
            parameters.Add("@IndustryCode", dto.IndustryCode);
            parameters.Add("@Currency", dto.Currency);

            await _db.ExecuteAsync(
                "sp_UpsertCompanyCore",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var companyId = parameters.Get<int>("@CompanyId");

            return Ok(new { CompanyId = companyId });
        }

        // PUT: api/company/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid company data.");

            dto.CompanyId = id; // force consistency

            return await CreateCompany(dto); // reuse same logic
        }

        private int GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}
