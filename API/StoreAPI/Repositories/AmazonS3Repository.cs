using Amazon.S3;
using Amazon.S3.Transfer;
using StoreAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Amazon.S3.Model;
using StoreAPI.Models;
using Azure.Core;
using Amazon.Runtime.SharedInterfaces;

namespace StoreAPI.Repositories
{
    public class AmazonS3Repository : IServiceS3
    {
        private readonly IAmazonS3 _amazonS3Client;
        private readonly IConfiguration _config;
        private readonly ILogger<AmazonS3Repository> log;
        private readonly string _awsAccessKey;
        private readonly string _awsSecretKey;

        // Inject the IAmazonS3 client (this can be configured in DI container)
        public AmazonS3Repository(IConfiguration config, IAmazonS3 amazonS3Client,ILogger<AmazonS3Repository> logs)
        {
            _awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            _awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            _config = config;
            log = logs;
            _amazonS3Client = amazonS3Client;
           // _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, awsSessionToken, RegionEndpoint.GetBySystemName(region));

        }

        public string GeneratePreSignedURL(string filename, int expirationMinutes = 15)
        {

            var awsCredentials = new BasicAWSCredentials(_awsAccessKey,_awsSecretKey);
            var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.APSouth1);
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _config["AWS:BucketName"],
                Key = filename,//here it is accepting the bill location as filename 
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
            };
            return s3Client.GetPreSignedURL(request);
        }
        

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            string bucketName = _config["AWS:BucketName"];
            string key = _awsAccessKey;

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = bucketName,
                ContentType = file.ContentType
            };

            var transferUtility = new TransferUtility(_amazonS3Client);
            await transferUtility.UploadAsync(uploadRequest);

            return $"https://{bucketName}.s3.amazonaws.com/{key}";
        }

        

        public async Task<S3FileDetailsDTO> UploadFileAsyncNew(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }

            log.LogInformation("Inside UploadFileAsyncNew Method in AmazonS3Repository");
            string bucketName = _config["AWS:BucketName"];
            string accessKeyId = _awsAccessKey;
            string secretAccessKey = _awsSecretKey;
            string region = _config["AWS:Region"];

            if (string.IsNullOrEmpty(bucketName) || string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(secretAccessKey)) { 
                log.LogWarning($"AWS credentials or BucketName are not configured.");
                throw new Exception("AWS credentials or BucketName are not configured.");
            }
            try
            {
                log.LogInformation($"File upload method started for S3\nFile will be uploaded in {bucketName} in AWS {region} region");
                // Initialize AWS credentials and S3 client
                var awsCredentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
                var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.GetBySystemName(region));

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                string uniqueKey = Guid.NewGuid() + "_" + file.FileName;

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = memoryStream,
                    Key = uniqueKey,
                    BucketName = bucketName,
                    ContentType = file.ContentType,
                    //CannedACL = S3CannedACL.PublicRead // Optional: Set file as public
                };

                var fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
                log.LogInformation("File uploaded to S3");
                var newFileDetails = new S3FileDetailsDTO()
                {
                    FileLocation= $"https://{bucketName}.s3.{region}.amazonaws.com/{uniqueKey}",
                    FileName= file.FileName,
                    UniqueName = uniqueKey
                };
                return (newFileDetails);
            }
            catch (AmazonS3Exception ex)
            {
                log.LogError(ex, "Some error happened while uploading to S3");
                var uploadedFileErrorDetail = new S3FileDetailsDTO()
                {
                    Message = ex.Message
                };
                return uploadedFileErrorDetail;
            }
           
        }


        // Example of DeleteFileAsync
        public async Task<bool> DeleteFileAsync(string bucketName, string key)
        {
            var deleteRequest = new Amazon.S3.Model.DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            var response = await _amazonS3Client.DeleteObjectAsync(deleteRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }

        // Example of DownloadFileAsync
        public async Task<Stream> DownloadFileAsync(string bucketName, string key)
        {
            var getRequest = new Amazon.S3.Model.GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            var response = await _amazonS3Client.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }
    }
}
