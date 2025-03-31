using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class BahiKhata
    {
        [Key]
        public int DueID { get; set; }
        public int CustomerId { get; set; }
        public string TotalBillAmount { get; set; }
        public string NewAmount { get; set; }
        public string PaidAmount { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FileLocation { get; set; }
        public string FileName { get; set; }
    }
}