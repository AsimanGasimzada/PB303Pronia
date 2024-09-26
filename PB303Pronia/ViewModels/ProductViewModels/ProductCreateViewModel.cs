namespace PB303Pronia.ViewModels;

public class ProductCreateViewModel
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Rating { get; set; }
    public IFormFile MainImage { get; set; } = null!;
    public IFormFile HoverImage { get; set; } = null!;
    public List<IFormFile> AdditionalImages { get; set; } = new();
}
