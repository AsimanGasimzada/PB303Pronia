using Microsoft.AspNetCore.Mvc.Rendering;

namespace PB303Pronia.ViewModels;

public class BlogUpdateViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Author { get; set; } = null!;
    public IFormFile? Image { get; set; } = null!;
    public List<SelectListItem> CategoryList { get; set; } = new();
    public List<int> CategoryIds { get; set; } = new();
}
