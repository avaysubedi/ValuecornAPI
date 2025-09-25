using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ValuecornAPI.Controllers
{
 
        [ApiController]
        [Route("api/lookups")]
        public class LookupsController : ControllerBase
        {
            private readonly IConfiguration _config;
            public LookupsController(IConfiguration config) { _config = config; }

            [HttpGet("countries")]
            public async Task<IActionResult> GetCountries()
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var result = await conn.QueryAsync("sp_GetCountryEquityRiskPremiums", commandType: CommandType.StoredProcedure);
                    return Ok(result);
                }
            }

            [HttpGet("industries")]
            public async Task<IActionResult> GetIndustries()
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var result = await conn.QueryAsync("sp_GetIndustries", commandType: CommandType.StoredProcedure);
                    return Ok(result);
                }
            }

            [HttpGet("ratings")]
            public async Task<IActionResult> GetRatings()
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var result = await conn.QueryAsync("sp_GetSyntheticRatings", commandType: CommandType.StoredProcedure);
                    return Ok(result);
                }
            }

            [HttpGet("countrypremiums")]
            public async Task<IActionResult> GetCountryPremiums()
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var result = await conn.QueryAsync(
                        "sp_GetCountryEquityRiskPremiums",
                        
                        commandType: CommandType.StoredProcedure
                    );
                    return Ok(result);
                }
            }
        }

    }
