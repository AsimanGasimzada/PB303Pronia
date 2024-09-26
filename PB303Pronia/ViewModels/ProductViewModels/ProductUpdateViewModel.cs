namespace PB303Pronia.ViewModels;

public class ProductUpdateViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Rating { get; set; }
    public IFormFile? MainImage { get; set; }
    public string? MainImagePath { get; set; }
    public IFormFile? HoverImage { get; set; }
    public string? HoverImagePath { get; set; }
    public List<IFormFile>   AdditionalImages { get; set; } = new();
    public List<string>? AdditionalImagePaths { get; set; } = new();
    public List<int> AdditionalImageIds { get; set; } = new();
}
