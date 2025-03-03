using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.SqlServer.Server;
using StoreAPI.Interfaces;
using StoreAPI.Models;
using System.Net;
using StoreAPI.Database;
using Amazon.Runtime.SharedInterfaces;
using Microsoft.AspNetCore.Authorization;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        #region Vairable Declarations
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IServiceS3 _aws;
        private readonly IAmazonS3 _awsclient;
        private readonly ILogger _log;
        private readonly DatabaseDetails _context;

        #endregion Vairable Declarations

        #region Constructor
        public UserController(ILogger<UserController> log, IUserRepository userRepository,IServiceS3 aws,IConfiguration config,IAmazonS3 awsclient,DatabaseDetails db) 
        {
            _context = db;
            _userRepository = userRepository;
            _configuration = config;
            _aws = aws;
            _awsclient = awsclient;
            _log = log;
        }

        #endregion Constructor

        #region Get Methods
        [AllowAnonymous]
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
        #endregion Get Methods

        #region Post Methods
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var result= await _userRepository.Login(login);
            if(result!=null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        #region RESET USER JIRA STOREREPO-14 END

        [AllowAnonymous]
        [HttpPost("reset-user")]
        public async Task<IActionResult> ResetPassword(ResetUser reset)
        {
            var result = await _userRepository.ResetUser(reset);
            if(result==null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("confirm-password")]
        public async Task<IActionResult> ConfirmPassword(ResetUser reset)
        {
            var result = await _userRepository.ConfirmForgetPassword(reset);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        #endregion RESET USER JIRA STOREREPO-14 END
        [AllowAnonymous]
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

        [HttpPost("upload-bill")]
        public async Task<IActionResult> UploadBill(IFormFile file)
        {
            var details= _aws.UploadFileAsync(file);
            return Ok(details);
        }
        [HttpPost("upload-bill-new")]
        public async Task<IActionResult> UploadDocumentToS3(IFormFile file)
        {
            Console.WriteLine("Inside upload-method-new");
            if (file is null || file.Length <= 0)
                return BadRequest("File is not provided or is empty.");

            try
            {
                Console.WriteLine("invoking actual method");
                var result = await _aws.UploadFileAsyncNew(file);
                return Ok(new { Message = "File uploaded successfully", FileUrl = result.FileLocation });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        /*    [HttpPost("AddCustomerDetails")]
            public async Task<IActionResult> AddCustomerDetails(CustomerDetailsDTO customer)
            {
                if(customer == null)
                {
                    return BadRequest("Data not available for customer");
                }

               *//* if(customer.FileLocation == null)
                {
                    return BadRequest("No file is there, please upload one");
                }*//*
                await _userRepository.AddCustomerDetails(customer);
                return Ok("File Added");

            }*/

        [HttpPost("AddCustomerDetails")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddCustomerDetails(CustomerDetailsDTO customer)
        {
            _log.LogInformation("Inside AddCustomerDetails Controller");
            if (customer == null)
            {
                _log.LogWarning("Customer data is missing");
                return BadRequest("Customer data is missing.");
            }

            if (customer.File == null || customer.File.Length == 0)
            {
                _log.LogWarning("File is missing. Please upload a valid file.");
                return BadRequest("File is missing. Please upload a valid file.");
            }

            try
            {
              var result=  await _userRepository.AddCustomerDetails(customer);

                if(!result.Contains("Customer details saved successfully."))
                {
                    _log.LogWarning($"{result.ToString()}");
                    return Conflict(new { message = result.ToString() });
                }
                return Ok("Customer details saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, "An error occurred");
            }
        }
        [HttpPost("submitForm")]
        public async Task<IActionResult> SubmitForm([FromForm] string name, [FromForm] string email, [FromForm] string description,  IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var bucketName = _configuration["AWS:BucketName"]; // Get bucket name from configuration
                var keyName = $"{Guid.NewGuid()}-{file.FileName}"; // Unique file name in S3

                using (var stream = file.OpenReadStream())
                {
                    var putObjectRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        InputStream = stream,
                        ContentType = file.ContentType
                    };

                    await _awsclient.PutObjectAsync(putObjectRequest);
                }

                var fileLocation = $"https://{bucketName}.s3.{_configuration["AWS:Region"]}.amazonaws.com/{keyName}"; // Construct S3 URL

                var formData = new FormData
                {
                    Name = name,
                    Email = email,
                    Description = description,
                    FileLocation = fileLocation
                };

                _context.FormData.Add(formData);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (AmazonS3Exception e)
            {
                return StatusCode(500, $"Error uploading to S3: {e.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion Post Methods

        #region Put Methods

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

        #endregion Put Methods

        #region Delete Methods
        #endregion Delete Methods
}
