namespace StoreAPI.Models
{
    public class AmazonS3DTO
    {
        public string? Prefix {  get; set; }
        public string? PresignedURL { get; set; }
    }
}
