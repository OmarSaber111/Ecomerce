using Ecomerce.Api.Controllers;
using Ecomerce.Core.DTOs.Product_DTOs;
using Ecomerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
           
            _mockRepo = new Mock<IProductRepository>();

           
            _controller = new ProductsController(_mockRepo.Object);
        }

        
        [Fact]
        public async Task AddProduct_ReturnsOk_WhenProductAdded()
        {
       
            var newProduct = new ProductCreateDto
            {
                Name = "Test Product",
                Price = 100,
                Category = "Test Category"
            };

            _mockRepo.Setup(r => r.AddProductAsync(newProduct))
                     .ReturnsAsync(true); 

          
            var result = await _controller.AddProduct(newProduct);

        
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Product added successfully", okResult.Value);
        }

       
        [Fact]
        public async Task AddProduct_ReturnsBadRequest_WhenAddFails()
        {
            
            var newProduct = new ProductCreateDto
            {
                Name = "Test Product",
                Price = 100,
                Category = "Test Category"
            };

            _mockRepo.Setup(r => r.AddProductAsync(newProduct))
                     .ReturnsAsync(false); 

            
            var result = await _controller.AddProduct(newProduct);

    
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add product", badRequest.Value);
        }
    }
}
