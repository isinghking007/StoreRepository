using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface ICommonRepository
    {
        Task<List<AmountPaid>> GetPaidAmountDetails(int customerID);
        Task<List<AmountDue>> GetDueAmountDetails(int customerID);
        Task<string> PaidAmountMethod(AmountPaidDTO dueAmount);
        Task<string> DueAmountMethod(AmountDueDTO dueAmount);
    }
}