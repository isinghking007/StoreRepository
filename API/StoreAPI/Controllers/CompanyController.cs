using Microsoft.AspNetCore.Mvc;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
       public CompanyController(ICompanyRepository companyRepository) 
       {
            _companyRepository=companyRepository;
       }
        [HttpGet("companyDetails/{companyId}")]
        public async Task<IActionResult> GetCompanyDetails(int companyId)
        {
            var company = await _companyRepository.GetCompanyDetails(companyId);
            if(company == null)
            {
                return NotFound("Company Details not found");
            }
            return Ok(company);
        }

        [HttpPost("addCompanyDetails")]
        public async Task<IActionResult> AddCompanyDetails(CompanyDetails company)
        {
            var companyDetails = await _companyRepository.AddCompanyDetails(company);
            if(companyDetails == null)
            {
                return NotFound("Company Not Found");
            }
            return Ok(companyDetails);
        }
    }
}
