using System.ComponentModel.DataAnnotations;

namespace DotnetCoding.Core.Models;

public class CreateProductModel
{
    [Required]
    public string Name { get; set; }
        
    [Range(0, 10000)]
    public int Price { get; set; }
}