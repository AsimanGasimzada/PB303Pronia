using PB303Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace PB303Pronia.Models;

public class Product:BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; } 

    [Range(0,5)]
    public int Rating { get; set; }
    public List<ProductImage> ProductImages { get; set; } = new();

}
