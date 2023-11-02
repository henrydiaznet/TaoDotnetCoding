using DotnetCoding.Core.Models;

namespace DotnetCoding.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDetails>> GetFilteredProductsAsync(SearchFilter filter,
            CancellationToken cancellationToken);

        Task<ProductDetails?> GetByIdAsync(int productId, CancellationToken cancellationToken);
        
        Task<ProductDetails> CreateProductAsync(CreateProductModel newProduct, CancellationToken cancellationToken);
        
        Task<ProductDetails> UpdateProductAsync(UpdateProductModel product, CancellationToken cancellationToken);
        
        Task DeleteProductAsync(ProductDetails product, CancellationToken cancellationToken);
    }
}
