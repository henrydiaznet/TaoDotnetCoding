using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;

namespace DotnetCoding.Services;

public class RequestService : IRequestService
{
    private readonly IUnitOfWork _unitOfWork;

    public RequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ProductRequest>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _unitOfWork.Requests.GetAllAsync(cancellationToken);
    }

    public async Task ProcessRequestAsync(RequestResolutionModel resolution, CancellationToken cancellationToken)
    {
        var request = await _unitOfWork.Requests.GetByIdAsync(resolution.RequestId, cancellationToken);
        if (request is null)
        {
            throw new Exception("Attempting to resolve non-existent request");
        }

        if (resolution.Action == RequestAction.Reject)
        {
            await HandleRejectActionAsync(request, cancellationToken);
        }
        else
        {
            await HandleApproveActionAsync(request, cancellationToken);
        }

        await _unitOfWork.Requests.DeleteAsync(request, cancellationToken);
    }

    private async Task HandleApproveActionAsync(ProductRequest request, CancellationToken cancellationToken)
    {
        switch (request.RequestType)
        {
            case ProductRequestType.CreateExpensive:
                request.Product.Status = ProductStatus.Active;
                await _unitOfWork.Products.UpdateAsync(request.Product, cancellationToken);
                break;
            case ProductRequestType.RaisePrice:
                request.Product.Price = request.SuggestNewPrice ?? request.Product.Price;
                request.Product.Status = ProductStatus.Active;
                await _unitOfWork.Products.UpdateAsync(request.Product, cancellationToken);
                break;
            case ProductRequestType.DeleteProduct:
                await _unitOfWork.Products.DeleteAsync(request.Product, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task HandleRejectActionAsync(ProductRequest request, CancellationToken cancellationToken)
    {
        switch (request.RequestType)
        {
            case ProductRequestType.CreateExpensive:
                await _unitOfWork.Products.DeleteAsync(request.Product, cancellationToken);
                break;
            case ProductRequestType.DeleteProduct:
                request.Product.Status = ProductStatus.Active;
                await _unitOfWork.Products.UpdateAsync(request.Product, cancellationToken);
                break;
            case ProductRequestType.RaisePrice:
                request.Product.Status = ProductStatus.Active;
                await _unitOfWork.Products.UpdateAsync(request.Product, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task ToDeleteAsync(ProductDetails product, CancellationToken cancellationToken)
    {
        var toDelete = new ProductRequest
        {
            RequestType = ProductRequestType.DeleteProduct,
            Product = product,
            CreatedOn = DateTimeOffset.Now,
        };

        await _unitOfWork.Requests.AddAsync(toDelete, cancellationToken);
        await _unitOfWork.SaveAsync();
    }

    public async Task ToCreateExpensiveProductAsync(ProductDetails product, CancellationToken cancellationToken)
    {
        var approval = new ProductRequest
        {
            RequestType = ProductRequestType.CreateExpensive,
            Product = product,
            CreatedOn = DateTimeOffset.Now,
        };
        
        await _unitOfWork.Requests.AddAsync(approval, cancellationToken);
        await _unitOfWork.SaveAsync();
    }

    public async Task ToOverpriceProductAsync(ProductDetails old, int suggestedPrice,
        CancellationToken cancellationToken)
    {
        var approval = new ProductRequest
        {
            RequestType = ProductRequestType.RaisePrice,
            ProductId = old.Id,
            SuggestNewPrice = suggestedPrice,
            CreatedOn = DateTimeOffset.Now,
        };
        
        await _unitOfWork.Requests.AddAsync(approval, cancellationToken);
        await _unitOfWork.SaveAsync();
    }

    public Task<ProductRequest?> GetByIdAsync(int requestResolutionRequestId, CancellationToken cancellationToken)
    {
        return _unitOfWork.Requests.GetByIdAsync(requestResolutionRequestId, cancellationToken);
    }
}