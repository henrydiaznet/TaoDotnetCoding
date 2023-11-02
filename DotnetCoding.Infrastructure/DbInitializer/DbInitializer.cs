using DotnetCoding.Core.Models;

namespace DotnetCoding.Infrastructure.DbInitializer;

public static class DbInitializer
{
    public static void Initialize(EshopContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var date = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);
        
        var products = new ProductDetails[]
        {
            new() {Name = "Woolen socks",     Price = 100,     Status = ProductStatus.Active,             UpdatedOn = date.AddDays(-20)},
            new() {Name = "Knitted gloves",   Price = 200,     Status = ProductStatus.Active,             UpdatedOn = date.AddDays(-19)},
            new() {Name = "Joke pants",       Price = 150,     Status = ProductStatus.Active,             UpdatedOn = date.AddDays(-18)},
            new() {Name = "Novelty scarf",    Price = 400,     Status = ProductStatus.UnderReview,        UpdatedOn = date.AddDays(-17)},
            new() {Name = "Serious things",   Price = 104,     Status = ProductStatus.Active,             UpdatedOn = date.AddDays(-16)},
            new() {Name = "Human clothes",    Price = 550,     Status = ProductStatus.Active,             UpdatedOn = date.AddDays(-15)},
            new() {Name = "Warm Beer",        Price = 150,     Status = ProductStatus.UnderReview,        UpdatedOn = date.AddDays(-14)},
            new() {Name = "Whatever",         Price = 140000,  Status = ProductStatus.UnderReview,        UpdatedOn = date.AddDays(-13)},
        };

        var requests = new ProductRequest[]
        {
            new() { RequestType = ProductRequestType.DeleteProduct, ProductId = 4, CreatedOn = date.AddDays(-1), Product = products[3] },
            new() { RequestType = ProductRequestType.RaisePrice, ProductId = 7, SuggestNewPrice = 1000, CreatedOn = date.AddDays(-2), Product = products[6]},
            new() { RequestType = ProductRequestType.CreateExpensive, ProductId = 8, CreatedOn = date.AddDays(-3), Product = products[7] },
        };
        
        context.Products.AddRange(products);
        context.Requests.AddRange(requests);
        
        context.SaveChanges();
    }
}