using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface ICommonRepository
    {
        Task<List<AllBorrowerDetails>> GetAllUserDueDetails();
        Task<List<BahiKhata>> GetBorrowerAmountDetails(int customerID);
        //Task<List<AmountDue>> GetDueAmountDetails(int customerID);
        Task<string> PaidAmountMethod(AmountPaidDTO dueAmount);
        Task<string> DueAmountMethod(AmountDueDTO dueAmount);
    }
}