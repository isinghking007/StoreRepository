using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{

    public class AmountPaid
    {
        [Key]
        public int PaidID { get; set; }
        public int CustomerId { get; set; }
        public string TotalAmount { get; set; }
        public string PaidAmount { get; set; }
        public string RemainingAmount { get; set; }
        public DateOnly ModifiedDate { get; set; }
        public string FileLocation { get; set; }
        public string FileName { get; set; }
    }
}