
using Ecomerce.Core.DTOs.Product_DTOs;
using Ecomerce.Core.Entities;
using Ecomerce.Core.Interfaces;
using Ecomerce.Core.IService;
using Ecomerce.Core.Sharing;
using Ecomerce.Infrastructure.Data.EcomerceData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Infrastructure.Repositories
{
    public class ProductRepository :Repository<Product>,  IProductRepository
    {
       
        private readonly IImageManagementService _imageManagementService;
        public ProductRepository(EcomerceDbContext context, IImageManagementService imageManagementService) : base(context)
        {
          
            _imageManagementService = imageManagementService;
        }

        public async Task<bool> AddProductAsync(ProductCreateDto addProduct)
        {
            if (addProduct == null) return false;

           
            var product = new Product
            {
                Category = addProduct.Category,
                ProductCode = addProduct.ProductCode,
                Name = addProduct.Name,
                Price = addProduct.Price,
                MinimumQuantity = addProduct.MinimumQuantity,
                DiscountRate = addProduct.DiscountRate
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            
            var imgPaths = await _imageManagementService.AddImgAsync(addProduct.Image, addProduct.Name);

            product.ImagePath = imgPaths.ToList();

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task DeleteProductAsync(Product product)
        {
            
            if (product.ImagePath != null)
            {
                foreach (var img in product.ImagePath)
                {
                     _imageManagementService.DeleteImgAsync(img);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }


        public async Task<ProductResponseDto> GetAllProduct(ProductParams productParams)
        {
            if (productParams.pagesize <= 0)
                productParams.pagesize = 3;

            const int MaxPageSize = 6;
            if (productParams.pagesize > MaxPageSize)
                productParams.pagesize = MaxPageSize;

            if (productParams.pagenumber <= 0)
                productParams.pagenumber = 1;

            var query = _context.Products.AsNoTracking();

            
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(m => searchWords.All(word =>
                    m.Name.ToLower().Contains(word.ToLower())));
            }

           
            if (!string.IsNullOrEmpty(productParams.sort))
            {
                query = productParams.sort switch
                {
                    "PriceAce" => query.OrderBy(p => p.Price),
                    "PriceDce" => query.OrderByDescending(p => p.Price),
                    _ => query.OrderBy(p => p.Name),
                };
            }

           
            var totalCount = await query.CountAsync();

          
            var products = await query
                .Skip(productParams.pagesize * (productParams.pagenumber - 1))
                .Take(productParams.pagesize)
                .ToListAsync();

           
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ProductCode = p.ProductCode,
                Price = p.Price,
                MinimumQuantity = p.MinimumQuantity,
                DiscountRate = p.DiscountRate,
                Category = p.Category,
                ImagePath = p.ImagePath
            }).ToList();

           
            var returnProductDto = new ProductResponseDto
            {
                Products = productDtos,
                PageNumber = productParams.pagenumber,
                PageSize = productParams.pagesize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)productParams.pagesize)
            };

            return returnProductDto;
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto updateProduct)
        {
            if (updateProduct == null) return false;

            var findproduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == updateProduct.Id);

            if (findproduct == null) return false;

            
            if (findproduct.ImagePath != null)
            {
                foreach (var oldImg in findproduct.ImagePath)
                {
                    _imageManagementService.DeleteImgAsync(oldImg);
                }
            }

            
            findproduct.ImagePath = new List<string>();
            findproduct.Category = updateProduct.Category ?? findproduct.Category;
            findproduct.Name = updateProduct.Name ?? findproduct.Name;
            findproduct.Price = updateProduct.Price;
            findproduct.MinimumQuantity = updateProduct.MinimumQuantity;
            findproduct.DiscountRate = updateProduct.DiscountRate;

            var imgPaths = await _imageManagementService.AddImgAsync(updateProduct.Image, updateProduct.Name);
            findproduct.ImagePath = imgPaths.ToList();  

            _context.Products.Update(findproduct);
            await _context.SaveChangesAsync();

            return true;
        }
       
        
    }
}
