using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class ProductDetails
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string PacketSize { get; set; }
        public double SalePrice { get; set; }
        public double PurchasePrice { get; set; }
        public int TotalVolume { get; set; }
        public int AvailableVolume { get; set; }

    }
}
