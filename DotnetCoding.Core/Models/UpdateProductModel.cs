using System.ComponentModel.DataAnnotations;

namespace DotnetCoding.Core.Models;

public class UpdateProductModel
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Range(0, 10000)]
    public int Price { get; set; }
}