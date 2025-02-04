using Microsoft.AspNetCore.Mvc;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _product;
        public ProductController(IProductRepository productRepository) 
        {
            _product = productRepository;
        }

        [HttpGet("getProductDetails")]
        public async Task<IActionResult> GetProductDetails(string productName)
        {
            var result=await _product.GetProductDetails(productName);
            return Ok(result);
        }
        [HttpGet("getProductFiles")]
        public async Task<IActionResult> GetProductFiles(string productName)
        {
            var result=await _product.GetAllFileDetails(productName);
            if(result ==null)
            {
                return BadRequest("Not Found");
            }
            return Ok(result);
        }

        [HttpPost("addproductdetails")]
        public async Task<IActionResult> AddProductDetails(ProductDetailDTO product)
        {
            var result = await _product.AddProductDetailsAsync(product);
            return Ok(result);
        }
    }
}
