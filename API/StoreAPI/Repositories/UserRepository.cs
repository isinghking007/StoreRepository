using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Database;
using StoreAPI.Interfaces;
using StoreAPI.Models;


namespace StoreAPI.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly DatabaseDetails _db;
        public UserRepository(DatabaseDetails db) 
        {
            _db = db;
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }
        public async Task<UserInfo> GetUserDetails(string phone)
        {
           UserInfo user= await _db.Users.FirstOrDefaultAsync(u=>u.MobileNumber == phone);  
            if (user == null)
            {
                return null;
            }
            return user;
        }
        public async Task<string> AddUserDetails(UserInfo userinfo)
        {
            UserInfo user = await GetUserDetails(userinfo.MobileNumber);
            if(user!=null)
            {
                return ("User Already Added");
            }
           await _db.Users.AddAsync(userinfo);
            await _db.SaveChangesAsync();
            return ("User details saved");
        }

        public async Task<UserInfo> UpdateUserDetails(string loginname, UserInfo updateUser)
        {
            
                UserInfo userInfo = await _db.Users.FirstOrDefaultAsync(u => u.UserName == loginname);
                if (userInfo == null)
                {
                    return null;
                }
                userInfo.FirstName = updateUser.FirstName;
                userInfo.LastName = updateUser.LastName;
                userInfo.MobileNumber = updateUser.MobileNumber;
                await _db.SaveChangesAsync();
                return userInfo; 

        }
    }
}
