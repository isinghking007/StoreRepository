using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using Amazon.Runtime.SharedInterfaces;
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

        #endregion Get Methods

        #region Post Methods
        
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