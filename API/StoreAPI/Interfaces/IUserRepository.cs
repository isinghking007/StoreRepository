using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<(string IdToken, DateTime ExpirationTime)> Login(Login login);
        Task<UserInfo> GetUserDetails(string phone);
        Task<string> AddUserDetails(UserInfo userInfo);
        Task<string> AddCustomerDetails(CustomerDetailsDTO customer);
        Task<List<UserInfo>> GetAllUsers();
        #region RESET USER JIRA STOREREPO-14 END
        Task<string> ResetUser(ResetUser reset);

        Task<string> ConfirmForgetPassword(ResetUser reset);

        #endregion RESET USER JIRA STOREREPO-14 END
        Task<UserInfo> UpdateUserDetails(string userId,UserInfo updateUser);
    }
}
