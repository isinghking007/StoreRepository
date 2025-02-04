using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class ProductDetails
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductMRP { get; set; }
        public string PacketSize { get; set; }
        public double SellingPrice { get; set; }
        public double PurchasePrice { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string BillLocation { get; set; }

        public string BillName { get; set;}
        public string UniqueKey { get; set; }
    }
}
