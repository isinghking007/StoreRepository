namespace StoreAPI.Models
{
    public class ProductDetailDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductMRP { get; set; }
        public string PacketSize { get; set; }
        public double SellingPrice { get; set; }
        public double PurchasePrice { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public IFormFile ProductBills { get; set; }
    }
}
