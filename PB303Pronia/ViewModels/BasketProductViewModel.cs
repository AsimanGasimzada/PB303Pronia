using PB303Pronia.Models;

namespace PB303Pronia.ViewModels;

public class BasketProductViewModel
{
    public Product Product { get; set; } = null!;
    public int Count { get; set; }
}