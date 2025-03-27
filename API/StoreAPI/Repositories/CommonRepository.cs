using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StoreAPI.Database;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly DatabaseDetails _db;
        private readonly ILogger<CommonRepository> _log;
        private readonly IServiceS3 _awsRepo;
        public CommonRepository(DatabaseDetails db,ILogger<CommonRepository> log,IServiceS3 awsRepo)
        {
            _awsRepo = awsRepo;
            _log = log;
            _db = db;
        }

        #region Get Methods
        public async Task<List<AmountDue>> GetDueAmountDetails(int customerID)
        {
            try
            {
                return await _db.AmountDue.Where(c => c.CustomerId == customerID).ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogError($"There seems to be an issue while getting the result from the DB {ex.Message}");
                return new List<AmountDue>();
            }
        }
        public async Task<List<AmountPaid>> GetPaidAmountDetails(int customerID)
        {
            try
            {
               return await _db.AmountPaid.Where(c => c.CustomerId == customerID).ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogError($"There seems to be an issue while getting the result from the DB {ex.Message}");
                return new List<AmountPaid>();
            }
        }

        #endregion Get Methods


        #region Post Methods
        public async Task<string> PaidAmountMethod(AmountPaidDTO dueAmount)
        {
            try
            {
                _log.LogInformation("Inside PaidAmountMethod in Common Repository");
                var customerExists = await _db.CustomerDetails.FirstOrDefaultAsync(c => c.CustomerId == dueAmount.CustomerId);
                if (customerExists == null)
                {
                    _log.LogWarning($"Details had not been found for CustomerId {dueAmount.CustomerId}, Please add the Customer Details first");
                    return ($"Details had not been found for CustomerId {dueAmount.CustomerId}, Please add the Customer Details first");
                }
                _log.LogInformation($"Customer with ID = {dueAmount.CustomerId} exists in DB and we are now uploading the file and saving details");

                var fileDetails = await _awsRepo.UploadFileAsyncNew(dueAmount.File);
                _log.LogInformation(fileDetails != null
       ? "File has been uploaded successfully in AWS S3"
       : $"File has not been uploaded because {fileDetails}");


                var amountDetails = new AmountPaid
                {
                    CustomerId = dueAmount.CustomerId,
                    TotalAmount = dueAmount.TotalAmount,
                    PaidAmount = dueAmount.PaidAmount,
                    RemainingAmount = dueAmount.RemainingAmount,
                    ModifiedDate = DateTime.UtcNow,
                    FileLocation = fileDetails?.FileLocation ?? "No File",
                    FileName = fileDetails?.FileName ?? "No File"
                };
                _log.LogInformation($"This amount details is going to be saved in AmountPaid table for PaidID= {amountDetails.PaidID}");
                await _db.AddAsync(amountDetails);
                await _db.SaveChangesAsync();
                _log.LogInformation($"This amount details has been saved in AmountPaid table for PaidID= {amountDetails.PaidID}");
                return $"Details saved successfully";
            }

            catch (Exception e)
            {
                _log.LogError($"An error occurred in DueAmountMethod: {e.Message}");
                return "An error occurred while processing your request.";
            }
        }

        public async Task<string> DueAmountMethod(AmountDueDTO dueAmount)
        {
            try
            {
               
                _log.LogInformation("Checking DB Connection");
                _log.LogInformation("Inside DueAmountMethod in Common Repository");
                var customerExists = await _db.CustomerDetails.FirstOrDefaultAsync(c => c.CustomerId == dueAmount.CustomerId);
                if (customerExists==null)
                {
                    _log.LogWarning($"Details had not been found for CustomerId {dueAmount.CustomerId}, Please add the Customer Details first");
                    return ($"Details had not been found for CustomerId {dueAmount.CustomerId}, Please add the Customer Details first");
                }
                _log.LogInformation($"Customer with ID = {dueAmount.CustomerId} exists in DB and we are now uploading the file and saving details");
           
                   var fileDetails = await _awsRepo.UploadFileAsyncNew(dueAmount.File);
                _log.LogInformation(fileDetails != null
       ? "File has been uploaded successfully in AWS S3"
       : $"File has not been uploaded because {fileDetails}");


                var amountDetails = new AmountDue
                {
                    CustomerId = dueAmount.CustomerId,
                    TotalBillAmount = dueAmount.TotalBillAmount,
                    PaidAmount = dueAmount.PaidAmount,
                    DueAmount = dueAmount.DueAmount,
                    ModifiedDate = DateTime.UtcNow,
                    FileLocation = fileDetails?.FileLocation ?? "No File",
                    FileName = fileDetails?.FileName ?? "No File"
                };
                _log.LogInformation($"This amount details is going to be saved in AmountDue table for DueId= {amountDetails.DueID}");
                await _db.AddAsync(amountDetails);
                await _db.SaveChangesAsync();
                _log.LogInformation($"This amount details has been saved in AmountDue table for DueId= {amountDetails.DueID}");
                return $"Details saved successfully";
            }
            
            catch (Exception e)
            {
                _log.LogError($"An error occurred in DueAmountMethod: {e.Message}");
                return "An error occurred while processing your request.";
            }
        }

        #endregion Post Methods
    }
}