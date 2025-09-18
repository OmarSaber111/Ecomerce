using Ecomerce.Core.DTOs.Product_DTOs;
using Ecomerce.Core.Entities;
using Ecomerce.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Core.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<ProductResponseDto> GetAllProduct(ProductParams productParams);

        Task<bool> AddProductAsync(ProductCreateDto addProduct);
        Task<bool> UpdateProductAsync(ProductUpdateDto updateProduct);
        Task DeleteProductAsync(Product product);
    }
}
