using NuGet.Protocol.Core.Types;
using PB303Pronia.Contexts;
using PB303Pronia.Models;
using PB303Pronia.Repositories.Abstractions;
using PB303Pronia.Repositories.Implementations.Generic;

namespace PB303Pronia.Repositories.Implementations;

public class SliderRepository : Repository<Slider>, ISliderRepository
{
    public SliderRepository(AppDbContext context) : base(context)
    {
    }
}
