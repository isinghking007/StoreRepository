using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Database;
using StoreAPI.Interfaces;
using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IServiceS3 serviceS3;
        private readonly IConfiguration _config;
        private readonly DatabaseDetails _db;

        public ProductRepository(IServiceS3 serviceS3,DatabaseDetails db,IConfiguration config)
        {
            this.serviceS3 = serviceS3;
            _config = config;
            _db = db;
            
        }
        public async Task<List<ProductDetails>> GetProductDetails(string productName)
        {
            var awsCredentials = new BasicAWSCredentials(_config["AWS:AccessKeyId"], _config["AWS:SecretAccessKey"]);
            var s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.APSouth1);

            // Fetch all matching records from the database
            var productDetailsList = await _db.ProductDetails
                .Where(p => p.ProductName.Contains(productName))
                .ToListAsync();
/*
            // Iterate through the list and generate pre-signed URLs for each item
            foreach (var productDetail in productDetailsList)
            {
                productDetail.BillLocation = serviceS3.GeneratePreSignedURL(productDetail.BillName);
            }

            return productDetailsList;*/
            // Map the result to a list of DTOs with BillLocation populated
            var result = productDetailsList.Select(productDetail => new ProductDetails
            {
                ProductName = productDetail.ProductName,
                BillLocation = serviceS3.GeneratePreSignedURL(productDetail.BillName)
            }).ToList();

            return result;
        }

        public async Task<List<string>> GetAllFileDetails(string productName)
        {
            productName = productName.ToLower();
            var productDetails =await _db.ProductDetails.Where(pf => pf.ProductName.Contains(productName)).
                OrderByDescending(p=>p.PurchaseDate).
                Select(pf =>new
                    {
                        pf.BillLocation,pf.BillName
                    }).ToListAsync();
            var s3FileDetails = new List<string>();
            if (productDetails ==null || !productDetails.Any())
            {
                string error = "No Details is there";
                s3FileDetails.Add(error); 
                return s3FileDetails;
            }

           
            foreach (var productDetail in productDetails)
            {
                var presingedURL = serviceS3.GeneratePreSignedURL(productDetail.BillLocation);
                s3FileDetails.Add(presingedURL);
            }
            return s3FileDetails;
        }

        public async Task<string> AddProductDetailsAsync(ProductDetailDTO products)
        {
            try
            {
                var filedetails = await serviceS3.UploadFileAsyncNew(products.ProductBills);

                var newProduct = new ProductDetails
                {
                    ProductName = products.ProductName,
                    PacketSize = products.PacketSize,
                    ProductMRP = products.ProductMRP,
                    SellingPrice = products.SellingPrice,
                    PurchasePrice = products.PurchasePrice,
                    TotalQuantity = products.TotalQuantity,
                    PurchaseDate = products.PurchaseDate,
                    BillLocation = filedetails.FileLocation,
                    BillName=filedetails.FileName,
                    UniqueKey=filedetails.UniqueName
                };
                await _db.ProductDetails.AddAsync(newProduct);
                await _db.SaveChangesAsync();
                return ("Product details added successfully");
            }
            catch(Exception e)
            {
                return e.Message.ToString();
            }
        }
    }
}
