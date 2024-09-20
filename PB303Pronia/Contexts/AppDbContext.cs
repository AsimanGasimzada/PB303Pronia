using Microsoft.EntityFrameworkCore;
using PB303Pronia.Models;
using System.Reflection;

namespace PB303Pronia.Contexts;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<Product> Products  { get; set; } = null!;
}
