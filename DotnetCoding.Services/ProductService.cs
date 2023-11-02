using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;

namespace DotnetCoding.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestService _requestService;

        public ProductService(IUnitOfWork unitOfWork, IRequestService requestService)
        {
            _unitOfWork = unitOfWork;
            _requestService = requestService;
        }

        public async Task<IEnumerable<ProductDetails>> GetFilteredProductsAsync(SearchFilter filter, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.GetFilteredAsync(filter, cancellationToken);
            return products.OrderByDescending(x => x.UpdatedOn);
        }

        public async Task<ProductDetails?> GetByIdAsync(int productId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.GetByIdAsync(productId, cancellationToken);
        }

        public async Task<ProductDetails> CreateProductAsync(CreateProductModel newProduct, CancellationToken cancellationToken)
        {
            var product = new ProductDetails
            {
                Status = ProductStatus.Active,
                Name = newProduct.Name,
                UpdatedOn = DateTimeOffset.Now, //I'm too lazy to inject ISystemClock or similar
                Price = newProduct.Price
            };

            if (product.Price > 5000)
            {
                product.Status = ProductStatus.UnderReview;
                await _requestService.ToCreateExpensiveProductAsync(product, cancellationToken);
                return product;
            }

            var result = await _unitOfWork.Products.CreateAsync(product, cancellationToken);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<ProductDetails> UpdateProductAsync(UpdateProductModel product, CancellationToken cancellationToken)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(product.Id, cancellationToken);
            if (existing is null)
            {
                throw new Exception("Attempting to update non-existent product");
            }

            existing.Name = product.Name;
            
            var oldPrice = existing.Price;
            if (oldPrice * 2 < product.Price || product.Price > 5000)
            {
                existing.Status = ProductStatus.UnderReview;
                await _requestService.ToOverpriceProductAsync(existing, product.Price, cancellationToken);
            }
            else
            {
                existing.Price = product.Price;
            }

            await _unitOfWork.Products.UpdateAsync(existing, cancellationToken);
            await _unitOfWork.SaveAsync();
            return existing;
        }

        public async Task DeleteProductAsync(ProductDetails product, CancellationToken cancellationToken)
        {
            await _requestService.ToDeleteAsync(product, cancellationToken);
        }
    }
}
