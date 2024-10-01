using PB303Pronia.Models;

namespace PB303Pronia.ViewModels;

public class HomeViewModel
{
    public List<Product> Products{ get; set; }=new List<Product>();
    public List<Slider> Sliders { get; set; } = new();
}
