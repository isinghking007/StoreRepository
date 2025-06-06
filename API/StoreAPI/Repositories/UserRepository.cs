﻿using Amazon;
using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
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
using StoreAPI.Service;


namespace StoreAPI.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly DatabaseDetails _db;
        private readonly IServiceS3 _awsService;
        private readonly ILogger<UserRepository> _log;
        private readonly AWSCognitoService _awsCognito;
        public UserRepository(DatabaseDetails db,IServiceS3 awsService,ILogger<UserRepository> log,AWSCognitoService aWSCognito) 
        {
            _db = db;
            _awsService = awsService;
            _log = log;
            _awsCognito = aWSCognito;
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }
        public async Task<UserInfo> GetUserDetails(string phone)
        {
            string phonenumber = FormatPhoneNumber(phone);
           UserInfo user= await _db.Users.FirstOrDefaultAsync(u=>u.MobileNumber == phonenumber);  
            if (user == null)
            {
                return null;
            }
            return user;
        }
        #region RESET USER JIRA STOREREPO-14 END
        public async Task<string> ResetUser(ResetUser reset)
        {
            _log.LogInformation("Inside Reset User Method in User Repository");
            var databasecheck = await _db.Users.FirstOrDefaultAsync(d => d.MobileNumber == reset.UserName);
            if (databasecheck != null)
            {
                var result = await _awsCognito.ResetUser(reset.UserName);

                if (result == null)
                {
                    _log.LogWarning($"Error while resetting password: {result}");
                    return $"Error while resetting password: {result}";
                }
                return result;
            }
            return $"User is not registered, Please SignUp";
        }

        public async Task<string> ConfirmForgetPassword(ResetUser reset)
        {
            _log.LogInformation("Inside Confirm Forget Password method in User Repository");
            var result = await _awsCognito.ConfirmForgotPassword(reset.UserName, reset.VerificationCode, reset.NewPassword);
            if (result == null)
            {
                _log.LogWarning($"Error while resetting password: {result}");
                return $"Error while resetting password: {result}";
            }
            return result;
        }

        #endregion RESET USER JIRA STOREREPO-14 END
        public async Task<string> AddUserDetails(UserInfo userinfo)
        {
            _log.LogInformation("Inside AddUserDetails Method in User Repository");
            UserInfo user = await GetUserDetails(userinfo.MobileNumber);
            if(user!=null)
            {
                return ("User Already Added");
            }
            var awscognito = await _awsCognito.RegisterUser(userinfo.Email,userinfo.MobileNumber, userinfo.UserName, userinfo.Password);
            if(awscognito == null)
            {
                _log.LogWarning($"Something wrong while adding user details into AWS Cognito = {awscognito}");
                return ("AWS Cognito Error");
            }
            _log.LogInformation($"AWS Cognioto value = {awscognito}");
           await _db.Users.AddAsync(userinfo);
            await _db.SaveChangesAsync();
            return ("User details saved");
        }

        public async Task<(string IdToken, DateTime ExpirationTime)> Login(Login login)
        {
            if (login.Email != null || login.Phone != null)
            {
                if (login.Email != null && login.Email != "")
                {
                    return await _awsCognito.LoginUser(login.Email, login.Password);
                }
                else
                {
                    return await _awsCognito.LoginUser(login.Phone, login.Password);
                }
            }
            throw new ArgumentException("Phone or Email is required");
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
    

    #region Helper Methods JIRA 17

    private string FormatPhoneNumber(string phoneNumber)
        {
            if(phoneNumber.Length==13 && phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.Substring(3, 10);
                return phoneNumber;
            }
            else if(phoneNumber.Length ==10 && !phoneNumber.StartsWith("+"))
            {
                return phoneNumber;
            }
            else
            {
                return "Invalid Phone Number";
            }
        }

    #endregion Helper Methods JIRA 17
}
}