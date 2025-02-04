using StoreAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class CustomerDetails
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
      
        public string Address { get; set; }
        public string Mobile { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int TotalAmount { get; set; }
        public int AmountPaid { get; set; }
        public int RemainingAmount { get; set; }
        public string FileLocation { get; set; }
        public string FileName { get; set; }


    }
}
