using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Core.DTOs.Product_DTOs
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public double DiscountRate { get; set; }
        public IFormFileCollection? Image { get; set; }
    }
}
