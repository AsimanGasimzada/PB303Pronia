using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public DbSet<ProductImage> ProductImages   { get; set; } = null!;
    public DbSet<Slider> Sliders   { get; set; } = null!;
    public DbSet<Blog> Blogs    { get; set; } = null!;
    public DbSet<Category> Categories     { get; set; } = null!;
}
