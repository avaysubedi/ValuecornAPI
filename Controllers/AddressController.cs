using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IDbConnection _db;

        public AddressController(IDbConnection db)
        {
            _db = db;
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertAddress([FromBody] CompanyAddress address)
        {
            if (address == null || address.CompanyId <= 0)
                return BadRequest("Invalid address data.");

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AddressId", address.AddressId, DbType.Int32, ParameterDirection.InputOutput);
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

                await _db.ExecuteAsync(
                    "sp_UpsertCompanyAddress",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var id = parameters.Get<int>("@AddressId");

                return Ok(new { AddressId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
