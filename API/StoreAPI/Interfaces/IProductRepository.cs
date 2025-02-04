using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<List<string>> GetAllFileDetails(string productName);
        Task<List<ProductDetails>> GetProductDetails(string productName);
         Task<string> AddProductDetailsAsync(ProductDetailDTO product);
    }
}
