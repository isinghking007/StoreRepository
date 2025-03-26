using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface ICommonRepository
    {
        Task<string> DueAmountMethod(AmountDueDTO dueAmount);
    }
}