using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface IServiceS3
    {
        string GeneratePreSignedURL(string filename, int expirationMinutes = 15);
        Task<string> UploadFileAsync(IFormFile file);
        Task<S3FileDetailsDTO> UploadFileAsyncNew(IFormFile file);

     /*   Task<bool> DeleteFileAsync(string bucketName, string key);
        Task<Stream> DownloadFileAsync(string bucketName, string key);
   */ }
}
