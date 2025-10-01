using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Controllers
{
    [ApiController]
    [Route("api/companyinfo")]
    public class CompanyInfoController : ControllerBase
    {
        private readonly IDbConnection _db;

        public CompanyInfoController(IDbConnection db)
        {
            _db = db;
        }

        [HttpPost("upsert-financials-tax")]
        public async Task<IActionResult> UpsertFinancialsAndTax([FromBody] CompanyInfoRequest request)
        {
            if (request == null || request.CompanyId <= 0)
                return BadRequest("Invalid company info data.");

            var userId = GetUserId();

            // ---- Financials ----
            var finParams = new DynamicParameters();
            finParams.Add("@FinancialId", request.Financials?.FinancialId, DbType.Int32, ParameterDirection.InputOutput);
            finParams.Add("@CompanyId", request.CompanyId);
            finParams.Add("@FiscalYearEnd", request.Financials?.FiscalYearEnd);
            finParams.Add("@AccountingStandards", request.Financials?.AccountingStandards);
            finParams.Add("@ExternalAuditor", request.Financials?.ExternalAuditor);
            finParams.Add("@UserId", userId);

            await _db.ExecuteAsync("sp_UpsertCompanyFinancials", finParams, commandType: CommandType.StoredProcedure);
            var financialId = finParams.Get<int>("@FinancialId");

            // ---- Tax ----
            var taxParams = new DynamicParameters();
            taxParams.Add("@TaxId", request.Tax?.TaxId, DbType.Int32, ParameterDirection.InputOutput);
            taxParams.Add("@CompanyId", request.CompanyId);
            taxParams.Add("@VATRegNo", request.Tax?.VATRegNo);
            taxParams.Add("@VATDate", request.Tax?.VATDate);
            taxParams.Add("@CorporateTaxRegNo", request.Tax?.CorporateTaxRegNo);
            taxParams.Add("@CorporateTaxDate", request.Tax?.CorporateTaxDate);
            taxParams.Add("@FreeZoneBenefit", request.Tax?.FreeZoneBenefit);
            taxParams.Add("@Exemptions", request.Tax?.Exemptions);
            taxParams.Add("@UserId", userId);

            await _db.ExecuteAsync("sp_UpsertCompanyTax", taxParams, commandType: CommandType.StoredProcedure);
            var taxId = taxParams.Get<int>("@TaxId");

            return Ok(new { FinancialId = financialId, TaxId = taxId });
        }

        private int GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }

    // request model
    public class CompanyInfoRequest
    {
        public int CompanyId { get; set; }
        public CompanyFinancialDto? Financials { get; set; }
        public CompanyTaxDto? Tax { get; set; }
    }
}
