using PB303Pronia.Models.Common;

namespace PB303Pronia.Models;

public class Category : BaseEntity
{
    public string Name { get; set; }=null!;
    public ICollection<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();

}
