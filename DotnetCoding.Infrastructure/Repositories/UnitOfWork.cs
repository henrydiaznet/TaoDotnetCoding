using DotnetCoding.Core.Interfaces;

namespace DotnetCoding.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EshopContext _dbContext;
        public IProductRepository Products { get; }
        public IRequestRepository Requests { get; }

        public UnitOfWork(EshopContext dbContext,
                            IProductRepository productRepository,
                            IRequestRepository requestRepository)
        {
            _dbContext = dbContext;
            Products = productRepository;
            Requests = requestRepository;
        }

        public Task<int> SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
