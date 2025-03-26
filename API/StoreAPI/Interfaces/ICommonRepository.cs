using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface ICommonRepository
    {
        Task<string> PaidAmountMethod(AmountPaidDTO dueAmount);
        Task<string> DueAmountMethod(AmountDueDTO dueAmount);
    }
}