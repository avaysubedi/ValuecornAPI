using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ValuecornAPI.Data;
using ValuecornAPI.Models;

namespace ValuecornAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompanyController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        // POST: api/company
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDto dto)
        {
            var userId = GetUserId(); // from JWT or session
            var companyId = await _companyRepo.UpsertCompanyAsync(dto, userId);
            return Ok(new { CompanyId = companyId });
        }

        // PUT: api/company/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyDto dto)
        {
            dto.CompanyId = id; // ensure consistency
            var userId = GetUserId();
            var companyId = await _companyRepo.UpsertCompanyAsync(dto, userId);
            return Ok(new { CompanyId = companyId });
        }

        private int GetUserId()
        {
            // Example: JWT claim "UserId"
            var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }



}

