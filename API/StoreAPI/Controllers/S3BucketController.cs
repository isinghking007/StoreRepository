using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAPI.Service;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3BucketController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _config;
        private readonly string _awsAccessKey;
        private readonly string _awsSecretKey;
        private readonly ILogger   _logger;
        private readonly AWSCognitoService _congnito;
        public S3BucketController(IAmazonS3 client,IConfiguration config,ILogger<S3BucketController> log,AWSCognitoService  congnito)
        {
            _awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            _awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            _s3Client = client;
            _config = config;
            _logger = log;
            _congnito = congnito;
        }

        [HttpGet("UserPoolDetails")]
        public async Task<IActionResult> GetUserPoolDetails()
        {
            _logger.LogInformation("Inside Get User Pool Details method");
            var result = await _congnito.ListUserPoolDetails();
            if(result == null)
            {
                return BadRequest("No Data found");
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuckets()
        {
            _logger.LogInformation($"S3Bucket Controller, GetAllBucket Method started");
            var awsCredentials = new BasicAWSCredentials(_awsAccessKey,_awsSecretKey);
            _logger.LogInformation($"AWS Credentials for testing: AWS Access Key = {_awsAccessKey}\nAWS Secret Key{_awsSecretKey}");
            var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.APSouth1);
            var buckets =await s3Client.ListBucketsAsync();
            var bucketnames=buckets.Buckets.Select(s=>s.BucketName).ToList();
            return Ok(bucketnames);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBucketAsyc(string bucketName)
        {
            _logger.LogInformation($"S3Bucket Controller, CreateBucketyAsync Method started");
            var awsCredentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
            _logger.LogInformation($"AWS Credentials for testing: AWS Access Key = {_awsAccessKey}\nAWS Secret Key{_awsSecretKey}");
            var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.APSouth1);
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistAsync(s3Client,bucketName);
            if (bucketExists)
            {
                return BadRequest($" Bucket {bucketName} already existed");
            }
            await s3Client.PutBucketAsync(bucketName);
            return Ok(bucketName);
        }

        [HttpDelete]
        
        public async Task<IActionResult> DeleteBucket(string bucketname)
        {
            var awsCredentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
            var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.APSouth1);
            await s3Client.DeleteBucketAsync(bucketname);
            return Ok($"Bucket {bucketname} had been deleted successfully");
        }
        [AllowAnonymous]
        [HttpPost("uploadFiles")]
        public async Task<IActionResult> UploadFiles(IFormFile files,string? prefix)
        {

            _logger.LogInformation($"S3Bucket Controller, UploadFiles Method started");
            var awsCredentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
            _logger.LogInformation($"AWS Credentials for testing: AWS Access Key = {_awsAccessKey}\nAWS Secret Key{_awsSecretKey}");
            var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.APSouth1);
            var request = new PutObjectRequest
            {
                BucketName = _config["AWS:BucketName"],
                Key = string.IsNullOrEmpty(prefix) ? files.FileName : $"{prefix.TrimEnd('/')}/{files.FileName}",
                InputStream = files.OpenReadStream()
            };
            request.Metadata.Add("content-type", files.ContentType);
            await s3Client.PutObjectAsync(request);
            return Ok($"File {prefix}/{files.FileName} had been uploaded successfully to S3 bucket {request.BucketName} ");
        }
        [AllowAnonymous]
        [HttpGet("preivewallfiles")]
        public async Task<IActionResult> GetAllFilesAsync(string? prefix, string bucketname)
        {
            try
            {
                _logger.LogInformation($"S3Bucket Controller, GetAllFilesAsync Method started");
                var awsCredentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
                _logger.LogInformation($"AWS Credentials for testing: AWS Access Key = {_awsAccessKey}\nAWS Secret Key{_awsSecretKey}");
                var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.APSouth1);
                var bucketexits=await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketname);
                if (!bucketexits)
                {
                    return NotFound($"Bucket {bucketname} doesn't exists");
                }
                var request = new ListObjectsV2Request()
                {
                    BucketName = bucketname,
                    Prefix = prefix
                };
                var result = await s3Client.ListObjectsV2Async(request);
                var s3objects = result.S3Objects.Select(obj =>
                {
                    var urlRequest = new GetPreSignedUrlRequest()
                    {
                        BucketName = obj.BucketName,
                        Key = obj.Key,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    };
                    return new AmazonS3DTO
                    {
                        Prefix = obj.Key,
                        PresignedURL = s3Client.GetPreSignedURL(urlRequest)
                    };

                });
                return Ok(s3objects);
            }
            catch(Exception e)
            {
                return Ok(e.Message);
            }
        }

    }
}
