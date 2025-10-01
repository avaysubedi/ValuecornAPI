using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;

namespace ValuecornAPI.Controllers
{
    [ApiController]
    [Route("api/lookups")]
    public class LookupsController : ControllerBase
    {
        private readonly IDbConnection _db;
        public LookupsController(IDbConnection db) => _db = db;

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var result = await _db.QueryAsync("sp_GetCountryEquityRiskPremiums", commandType: CommandType.StoredProcedure);
            return Ok(result);
        }

        [HttpGet("industries")]
        public async Task<IActionResult> GetIndustries()
        {
            var result = await _db.QueryAsync("sp_GetIndustries", commandType: CommandType.StoredProcedure);
            return Ok(result);
        }

        [HttpGet("ratings")]
        public async Task<IActionResult> GetRatings()
        {
            var result = await _db.QueryAsync("sp_GetSyntheticRatings", commandType: CommandType.StoredProcedure);
            return Ok(result);
        }

        [HttpGet("countrypremiums")]
        public async Task<IActionResult> GetCountryPremiums()
        {
            var result = await _db.QueryAsync("sp_GetCountryEquityRiskPremiums", commandType: CommandType.StoredProcedure);
            return Ok(result);
        }
    }
}
