namespace DotnetCoding.Core.Models
{
    public class ProductDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ProductStatus Status { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
