using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Database;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public class CompanyRepository:ICompanyRepository
    {
        private readonly DatabaseDetails _db;
        public CompanyRepository(DatabaseDetails db)
        {
            _db = db;
        }

        public async Task<CompanyDetails> GetCompanyDetails(int companyId)
        {
            var company = await _db.CompanyDetails.FirstOrDefaultAsync(c => c.CompanyID == companyId);
            if (company == null)
            {
                return null;
            }
            return company;
        }
        public async Task<string> AddCompanyDetails(CompanyDetails company)
        {
            var companyDetails = await _db.CompanyDetails.FirstOrDefaultAsync(c => c.CompanyName == company.CompanyName);
            if (companyDetails != null)
            {
                return "Company Details not found";
            }
            await _db.CompanyDetails.AddAsync(company);
            await _db.SaveChangesAsync();
            return "Company Details saved";
        }
    }

}
