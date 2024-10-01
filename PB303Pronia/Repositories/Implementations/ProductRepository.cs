using PB303Pronia.Contexts;
using PB303Pronia.Models;
using PB303Pronia.Repositories.Abstractions;
using PB303Pronia.Repositories.Implementations.Generic;

namespace PB303Pronia.Repositories.Implementations;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}
