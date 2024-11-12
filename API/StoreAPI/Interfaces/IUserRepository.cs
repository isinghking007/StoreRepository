using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<UserInfo> GetUserDetails(string phone);
        Task<string> AddUserDetails(UserInfo userInfo);
        Task<List<UserInfo>> GetAllUsers();

        Task<UserInfo> UpdateUserDetails(string userId,UserInfo updateUser);
    }
}
