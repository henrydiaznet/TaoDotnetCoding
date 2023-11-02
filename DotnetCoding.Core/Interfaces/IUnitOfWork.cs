

namespace DotnetCoding.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IRequestRepository Requests { get; }

        Task<int> SaveAsync();
    }
}
