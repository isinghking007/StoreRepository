using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class AmountDueDTO
    {
       
        public int CustomerId { get; set; }
        public string TotalBillAmount { get; set; }
        public string PaidAmount { get; set; }
        public string DueAmount { get; set; }
        public IFormFile? File { get; set; }
    }
}