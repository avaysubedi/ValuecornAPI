using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShareholdersController : ControllerBase
    {
        private readonly IDbConnection _db;

        public ShareholdersController(IDbConnection db)
        {
            _db = db;
        }

        // ============================
        // 1. Add Temp Shareholder
        // ============================
        [HttpPost("temp/add")]
        public async Task<IActionResult> AddTempShareholder([FromBody] ShareholderDto sh, [FromQuery] int companyId)
        {
            if (companyId <= 0) return BadRequest("Invalid company ID.");
            if (string.IsNullOrWhiteSpace(sh.ShareholderName)) return BadRequest("Shareholder name is required.");

            await _db.ExecuteAsync("sp_InsertTempShareholder",
                new
                {
                    CompanyId = companyId,
                    sh.ShareholderName,
                    sh.Nationality,
                    sh.Percentage,
                    sh.IsUBO,
                    sh.UBOId,
                    sh.Type
                },
                commandType: CommandType.StoredProcedure);

            return Ok(new { Message = "Temp shareholder added." });
        }

        // ============================
        // 2. Delete Temp Shareholder
        // ============================
        [HttpDelete("temp/delete/{companyId:int}/{tempId:int}")]
        public async Task<IActionResult> DeleteTempShareholder(int companyId, int tempId)
        {
            var rows = await _db.ExecuteAsync(
                "sp_DeleteTempShareholderById",
                new { CompanyId = companyId, TempId = tempId },
                commandType: CommandType.StoredProcedure);

            if (rows == 0)
                return NotFound("Temp shareholder not found.");

            return Ok(new { Message = "Temp shareholder deleted." });
        }

        // ============================
        // 3. Get Temp Shareholders
        // ============================
        [HttpGet("temp/{companyId:int}")]
        public async Task<IActionResult> GetTempShareholders(int companyId)
        {
            var result = await _db.QueryAsync<ShareholderTempDto>(
                @"SELECT TempId, CompanyId, ShareholderName, Nationality, Percentage, IsUBO, UBOId, Type, CreatedAt
                  FROM TmpCompanyShareholders 
                  WHERE CompanyId = @CompanyId",
                new { CompanyId = companyId });

            return Ok(result);
        }

        // ============================
        // 4. Commit Shareholders
        // ============================
        [HttpPost("commit/{companyId:int}")]
        public async Task<IActionResult> CommitShareholders(int companyId)
        {
            if (companyId <= 0) return BadRequest("Invalid company ID.");

            await _db.ExecuteAsync("sp_CommitCompanyShareholders",
                new { CompanyId = companyId },
                commandType: CommandType.StoredProcedure);

            return Ok(new { Message = "Shareholders committed successfully." });
        }
    }


  
}
