using DotnetCoding.Core.Models;

namespace DotnetCoding.Core.Interfaces;

public interface IRequestRepository
{
    Task<ProductRequest> AddAsync(ProductRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyList<ProductRequest>> GetAllAsync(CancellationToken cancellationToken);

    Task<ProductRequest?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task DeleteAsync(ProductRequest request, CancellationToken cancellationToken);
}
