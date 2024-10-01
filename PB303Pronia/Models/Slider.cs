using PB303Pronia.Models.Common;

namespace PB303Pronia.Models;

public class Slider : BaseEntity
{
    public string Title { get; set; } = null!;
    public string SubTitle { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}
