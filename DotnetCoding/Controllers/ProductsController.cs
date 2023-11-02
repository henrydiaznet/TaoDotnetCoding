using DotnetCoding.Core.Models;
using Microsoft.AspNetCore.Mvc;
using DotnetCoding.Services.Interfaces;

namespace DotnetCoding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        
        /// <summary>
        /// Get list of products
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] SearchFilter? filter, CancellationToken cancellationToken)
        {
            var productDetailsList = await _productService.GetFilteredProductsAsync(filter, cancellationToken);
            
            if(!productDetailsList.Any())
            {
                return NotFound();
            }
            
            return Ok(productDetailsList);
        }
        
        /// <summary>
        /// Creates a new Product
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductModel newProduct, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _productService.CreateProductAsync(newProduct, cancellationToken);
            return Ok(result);
        }
        
        /// <summary>
        /// Creates a new Product
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody]UpdateProductModel product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existing = await _productService.GetByIdAsync(product.Id, cancellationToken);
            if (existing is null)
            {
                return NotFound();
                //var newProduct = new CreateProductModel
                //{
                //    Name = product.Name,
                //    Price = product.Price
                //};
                //
                //var create = await _productService.CreateProductAsync(newProduct, cancellationToken);
                //return Created(@"//products", create);
            }

            var result = await _productService.UpdateProductAsync(product, cancellationToken);
            return Ok(result);
        }
        
        /// <summary>
        /// Deletes a Product
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var existing = await _productService.GetByIdAsync(productId, cancellationToken);
            if (existing is null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(existing, cancellationToken);
            return Ok();
        }
    }
}
