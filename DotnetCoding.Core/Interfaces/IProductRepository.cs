using DotnetCoding.Core.Models;

namespace DotnetCoding.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<ProductDetails>> GetAllAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<ProductDetails>> GetFilteredAsync(SearchFilter? filter, CancellationToken cancellationToken);
        Task<ProductDetails> UpdateAsync(ProductDetails product, CancellationToken cancellationToken);
        Task<ProductDetails> CreateAsync(ProductDetails product, CancellationToken cancellationToken);
        Task DeleteAsync(ProductDetails product, CancellationToken cancellationToken);
        Task<ProductDetails?> GetByIdAsync(int productId, CancellationToken cancellationToken);
    }
}
