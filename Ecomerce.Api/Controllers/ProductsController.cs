using Ecomerce.Core.DTOs.Product_DTOs;
using Ecomerce.Core.Interfaces;
using Ecomerce.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
           
            [HttpPost("Add-Product")]
            public async Task<IActionResult> AddProduct([FromForm] ProductCreateDto addProductDto)
            {
                try
                {
                    var result = await _productRepository.AddProductAsync(addProductDto);
                    if (!result) return BadRequest("Failed to add product");
                    return Ok("Product added successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

         
            [HttpPut("Update-Product/{id}")]
            public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto updateProductDto)
            {
                try
                {
                    if (id != updateProductDto.Id) return BadRequest("Product ID mismatch");

                    var result = await _productRepository.UpdateProductAsync(updateProductDto);
                    if (!result) return NotFound("Product not found");

                    return Ok("Product updated successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpDelete("Delete-Product/{id}")]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                try
                {
                    var product = await _productRepository.GetByIdAsync(id);
                    if (product == null) return NotFound("Product not found");

                    await _productRepository.DeleteProductAsync(product);
                    return Ok("Product deleted successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("Get-All-Products")]
            public async Task<IActionResult> GetAllProducts([FromQuery] ProductParams productParams)
            {
                try
                {
                    var products = await _productRepository.GetAllProduct(productParams);
                    return Ok(products);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

          
            [HttpGet("Get-Product/{id}")]
            public async Task<IActionResult> GetProductById(int id)
            {
                try
                {
                    var product = await _productRepository.GetByIdAsync(id);
                    if (product == null) return NotFound("Product not found");
                    return Ok(product);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

