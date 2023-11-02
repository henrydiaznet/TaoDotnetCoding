using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoding.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EshopContext _dbContext;

        public ProductRepository(EshopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<ProductDetails>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<ProductDetails>()
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<ProductDetails>> GetFilteredAsync(SearchFilter? filter, CancellationToken cancellationToken)
        {
            if (filter is null)
            {
                return await GetAllAsync(cancellationToken);
            }
            
            var query = _dbContext.Set<ProductDetails>()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query
                    .Where(x => EF.Functions.Like(x.Name, $"%{filter.Name}%"));
            }

            if (filter.PriceRange?.Lower is not null)
            {
                query = query
                    .Where(x => x.Price >= filter.PriceRange.Lower);
            }
            
            if (filter.PriceRange?.Upper is not null)
            {
                query = query
                    .Where(x => x.Price <= filter.PriceRange.Upper);
            }

            if (filter.CreatedRange?.Lower is not null)
            {
                query = query
                    .Where(x => x.UpdatedOn >= filter.CreatedRange.Lower);
            }
            
            if (filter.CreatedRange?.Upper is not null)
            {
                query = query
                    .Where(x => x.UpdatedOn <= filter.CreatedRange.Upper);
            }

            return await query
                .Where(x => x.Status == ProductStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductDetails> UpdateAsync(ProductDetails product, CancellationToken cancellationToken)
        {
            product.UpdatedOn = DateTimeOffset.Now;
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<ProductDetails> CreateAsync(ProductDetails product, CancellationToken cancellationToken)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task DeleteAsync(ProductDetails product, CancellationToken cancellationToken)
        {
            product.Status = ProductStatus.Deleted;
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ProductDetails?> GetByIdAsync(int productId, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);
        }
    }
}
