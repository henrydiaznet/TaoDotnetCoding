namespace DotnetCoding.Core.Models;

public class ProductRequest
{
    public int Id { get; set; }
    public ProductRequestType RequestType { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public int? SuggestNewPrice { get; set; }
    public int ProductId { get; set; }
    public ProductDetails Product { get; set; }
}