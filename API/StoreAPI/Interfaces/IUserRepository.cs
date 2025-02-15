using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<string> Login(Login login);
        Task<UserInfo> GetUserDetails(string phone);
        Task<string> AddUserDetails(UserInfo userInfo);
        Task<string> AddCustomerDetails(CustomerDetailsDTO customer);
        Task<List<UserInfo>> GetAllUsers();

        Task<UserInfo> UpdateUserDetails(string userId,UserInfo updateUser);
    }
}
