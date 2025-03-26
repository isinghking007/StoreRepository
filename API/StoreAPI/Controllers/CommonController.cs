using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CommonController : ControllerBase
    {
        public readonly ICommonRepository _commonRepo;
        public CommonController(ICommonRepository commonRepository)
        {
            _commonRepo = commonRepository;
        }

        #region Get Methods

        #endregion Get Methods

        #region Post Methods
        
        [AllowAnonymous]
        [HttpPost("addDueAmount")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> TestMethod(AmountDueDTO cust)
        {
            if(cust!=null)
            {
                var result = await  _commonRepo.DueAmountMethod(cust);
                return Ok(result);
            }
            return BadRequest();
        }

        #endregion Post Methods
    }
}