using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Controllers
{
 
        [ApiController]
        [Route("api/company")]
        public class CompanyController : ControllerBase
        {
            private readonly IConfiguration _config;

            public CompanyController(IConfiguration config)
            {
                _config = config;
            }

            // INSERT / UPDATE
            [HttpPost]
            [HttpPut("{id?}")]
            public async Task<IActionResult> UpsertCompany([FromBody] Company company, int? id = null)
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", id);
                    parameters.Add("@CompanyCode", company.CompanyCode);
                    parameters.Add("@CompanyName", company.CompanyName);
                    parameters.Add("@CountryCode", company.CountryCode);
                    parameters.Add("@IndustryCode", company.IndustryCode);
                    parameters.Add("@Currency", company.Currency);
                    parameters.Add("@FiscalYearEnd", company.FiscalYearEnd);
                    parameters.Add("@SharesOutstanding", company.SharesOutstanding);
                    parameters.Add("@NetDebt", company.NetDebt);
                    parameters.Add("@CashBalance", company.CashBalance);

                    var result = await conn.QuerySingleAsync<int>(
                        "sp_UpsertCompany",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return Ok(new { CompanyId = result });
                }
            }

            // GET BY ID
            [HttpGet("{id}")]
            public async Task<IActionResult> GetCompanyById(int id)
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var result = await conn.QuerySingleOrDefaultAsync<Company>(
                        "SELECT * FROM dbo.Companies WHERE CompanyId = @Id",
                        new { Id = id }
                    );

                    if (result == null) return NotFound();
                    return Ok(result);
                }
            }

            // GET ALL
            [HttpGet]
            public async Task<IActionResult> GetCompanies()
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var result = await conn.QueryAsync<Company>(
                        "SELECT * FROM dbo.Companies ORDER BY CompanyName"
                    );
                    return Ok(result);
                }
            }
        }


    }

