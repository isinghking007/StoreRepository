namespace StoreAPI.Models
{
    public class AmountPaidDTO
    {
        public int CustomerId { get; set; }
        public string TotalAmount { get; set; }
        public string PaidAmount { get; set; }
        public string RemainingAmount { get; set; }
        public IFormFile? File { get; set; }
    }
}