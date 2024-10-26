using Microsoft.AspNetCore.Mvc;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("test")]
        public async Task<string> Test()
        {
            await Task.Delay(10);
            return "hello from async method";
        }
    }
}
