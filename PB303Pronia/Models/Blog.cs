using PB303Pronia.Models.Common;

namespace PB303Pronia.Models;

public class Blog : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public ICollection<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();


}
