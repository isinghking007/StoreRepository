using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{

    public class AmountDue
    {
        [Key]
        public int DueID { get; set; }
        public int CustomerId { get; set; }
        public string TotalBillAmount { get; set; }
        public string PaidAmount { get; set; }
        public string DueAmount { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FileLocation { get; set; }
        public string FileName { get; set; }
    }
}