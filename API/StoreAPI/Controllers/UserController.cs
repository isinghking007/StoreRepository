using Microsoft.AspNetCore.Mvc;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet("allUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            if(users==null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("userDetails/{phone}")]
        public async Task<IActionResult> GetUserDetails(string phone)
        {
            var userDetails = await _userRepository.GetUserDetails(phone);
            if(userDetails ==null)
            {
                return Ok("Error");
            }
            return Ok(userDetails);
        }

        [HttpPost("AddUserDetails")]
        public async Task<IActionResult> AddUserDetails(UserInfo userinfo)
        {
            if(userinfo == null)
            {
                return BadRequest("Data not available");
            }
            await _userRepository.AddUserDetails(userinfo);
            return CreatedAtAction(nameof(GetUserDetails), new { phone = userinfo.MobileNumber }, userinfo);
        }

        [HttpPut("updateUser/{userId}")]

        public async Task<IActionResult> UpdateUserDetails(string userId, [FromBody]UserInfo updateUser)
        {
            var userDetails = await _userRepository.UpdateUserDetails(userId, updateUser);
            if(userDetails == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(userDetails);
        }
    }
}
