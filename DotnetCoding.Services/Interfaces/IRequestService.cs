using DotnetCoding.Core.Models;

namespace DotnetCoding.Services.Interfaces;

public interface IRequestService
{
    Task<IReadOnlyList<ProductRequest>> GetAllAsync(CancellationToken cancellationToken);
    Task ProcessRequestAsync(RequestResolutionModel resolution, CancellationToken cancellationToken);
    Task ToDeleteAsync(ProductDetails product, CancellationToken cancellationToken);
    Task ToCreateExpensiveProductAsync(ProductDetails product, CancellationToken cancellationToken);
    Task ToOverpriceProductAsync(ProductDetails old, int suggestedPrice,
        CancellationToken cancellationToken);
    Task<ProductRequest?> GetByIdAsync(int requestResolutionRequestId, CancellationToken cancellationToken);
}