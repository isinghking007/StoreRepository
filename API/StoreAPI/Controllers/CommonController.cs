using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using Amazon.Runtime.SharedInterfaces;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonRepository _commonRepo;
        private readonly ILogger<CommonController> _log;
        public CommonController(ICommonRepository commonRepository,ILogger<CommonController>log)
        {
            _commonRepo = commonRepository;
            _log = log;
        }

        #region Get Methods
        [AllowAnonymous]
        [HttpGet("allUserDueDetails")]
        public async Task<IActionResult> GetAllUserDueDetails()
        {
            try
            {
                var result = await _commonRepo.GetAllUserDueDetails();
                return Ok(result);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("paidAmountDetails/{customerID}")]
        public async Task<IActionResult> GetPaidAmountDetails(int customerID)
        {
            try
            {
                if (customerID > 0)
                {
                    var result =await _commonRepo.GetBorrowerAmountDetails(customerID);
                    return Ok(result);
                }
                return BadRequest("Invalid data.");
            }
            catch(Exception ex)
            {
                _log.LogError($"Error in the GetPaidAmountDetails Method {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        //[AllowAnonymous]
        //[HttpGet("dueAmountDetails/{customerID}")]
        //public async Task<IActionResult> GetDueAmountDetails(int customerID)
        //{
        //    try
        //    {
        //        if (customerID > 0)
        //        {
        //            var result = await _commonRepo.GetDueAmountDetails(customerID);
        //            return Ok(result);
        //        }
        //        return BadRequest("Invalid data.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError($"Error in the GetPaidAmountDetails Method {ex.Message}");
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}

        #endregion Get Methods

        #region Post Methods

        [AllowAnonymous]
        [HttpPost("addPaidAmount")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddPaidAmount(AmountPaidDTO cust)
        {
            try
            {
                if (cust != null)
                {
                    var result = await _commonRepo.PaidAmountMethod(cust);
                    return Ok(result);
                }
                return BadRequest("Invalid data.");
            }
            catch (Exception ex)
            {
                _log.LogError($"An error occurred in AddDueAmount: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        [AllowAnonymous]
        [HttpPost("addDueAmount")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddDueAmount(AmountDueDTO cust)
        {
            try
            {
                if (cust != null)
                {
                    var result = await _commonRepo.DueAmountMethod(cust);
                    return Ok(result);
                }
                return BadRequest("Invalid data.");
            }
            catch (Exception ex)
            {
                _log.LogError($"An error occurred in AddDueAmount: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        #endregion Post Methods
    }
}