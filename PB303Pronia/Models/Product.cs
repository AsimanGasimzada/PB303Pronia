using System.ComponentModel.DataAnnotations;

namespace PB303Pronia.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; } 
    public string ImagePath { get; set; } = null!;
    public string HoverImagePath { get; set; } = null!;

    [Range(0,5)]
    public int Rating { get; set; }
}
