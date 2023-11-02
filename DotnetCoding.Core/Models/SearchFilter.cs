namespace DotnetCoding.Core.Models;

public class SearchFilter
{
    public string? Name { get; set; }
    public PriceRange? PriceRange{ get; set; }
    public CreatedOnRange? CreatedRange { get; set; }
}