using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class DueDetails
    {
        [Key]
        public int DueId { get; set; }
        [Required]
        public int CustomerId { get; set; }
       
        public string BillNumber { get; set; }

        public string TotalAmount { get; set; }
        public string DueAmount { get; set; }
        public string PaidAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
