using Amazon.Runtime.SharedInterfaces;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Serilog.Core;
using StoreAPI.Database;
using StoreAPI.Interfaces;
using StoreAPI.Models;


namespace StoreAPI.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly DatabaseDetails _db;
        private readonly IServiceS3 _awsService;
        private readonly ILogger<UserRepository> _log;
        public UserRepository(DatabaseDetails db,IServiceS3 awsService,ILogger<UserRepository> log) 
        {
            _db = db;
            _awsService = awsService;
            _log = log;
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

        public async Task<string> AddCustomerDetails(CustomerDetailsDTO customer)
        {
            /*            CustomerDetails cust = await _db.CustomerDetails.FirstOrDefaultAsync(c => c.Mobile == customer.Mobile);
                        if (cust!=null)
                        {
                            return ("Customer Already Added");
                        }
                        var fileDetails = await _awsService.UploadFileAsyncNew(customer.File);
                       *//* customer.UploadedFileDetails = new FileHandle
                        {
                            FileName = customer.File.FileName,
                            FileLocation = fileDetails
                        };*//*
                        var customerdetail = new CustomerDetails
                        {
                            CustomerName = customer.CustomerName,
                            Mobile = customer.Mobile,
                            Address = customer.Address,
                            TotalAmount = customer.TotalAmount,
                            AmountPaid = customer.AmountPaid,
                            RemainingAmount = customer.RemainingAmount,
                            PurchaseDate = customer.PurchaseDate,
                            FileLocation = fileDetails // Save file location
                        };
                        _db.CustomerDetails.Add(customerdetail);
                        await _db.SaveChangesAsync();*/

            // Check if customer already exists
            try
            {
                _log.LogInformation("Inside AddCustomerDetails Method");
                var existingCustomer = await _db.CustomerDetails.FirstOrDefaultAsync(c => c.Mobile == customer.Mobile);
                if (existingCustomer != null)
                {
                    _log.LogInformation("Customer Already Added");
                    return ("Customer already added.");
                }

                // Upload file to AWS S3
                _log.LogInformation("Upload file to AWS S3 Method called");
                var fileLocation = await _awsService.UploadFileAsyncNew(customer.File);
                _log.LogInformation("File uploaded successfully");
                // Map DTO to entity
                var customerDetails = new CustomerDetails
                {
                    CustomerName = customer.CustomerName,
                    Mobile = customer.Mobile,
                    Address = customer.Address,
                    TotalAmount = customer.TotalAmount,
                    AmountPaid = customer.AmountPaid,
                    RemainingAmount = customer.RemainingAmount,
                    PurchaseDate = customer.PurchaseDate,
                    FileLocation = fileLocation.FileLocation,
                    FileName=fileLocation.FileName
                    // Save file location from S3
                };

                // Save to database
                _db.CustomerDetails.Add(customerDetails);
                await _db.SaveChangesAsync();

                return ("Customer Details saved successfully");
            }
            catch(Exception e)
            {
                _log.LogError(e, "Expcetion occured in this method");
                return $"some error happened {e}";
            }
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
