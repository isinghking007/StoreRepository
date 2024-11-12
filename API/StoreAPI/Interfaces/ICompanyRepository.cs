using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface ICompanyRepository
    {
        Task<CompanyDetails> GetCompanyDetails(int companyId);
        Task<string> AddCompanyDetails(CompanyDetails companyDetails);
    }
}
