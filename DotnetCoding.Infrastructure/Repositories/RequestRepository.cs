using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoding.Infrastructure.Repositories;

public class RequestRepository: IRequestRepository
{
    private readonly EshopContext _dbContext;

    public RequestRepository(EshopContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ProductRequest> AddAsync(ProductRequest request, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(request, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return request;
    }

    public async Task<IReadOnlyList<ProductRequest>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Requests
            .Include(x => x.Product)
            .OrderBy(x => x.CreatedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Requests
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task DeleteAsync(ProductRequest request, CancellationToken cancellationToken)
    {
        _dbContext.Requests.Remove(request);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}