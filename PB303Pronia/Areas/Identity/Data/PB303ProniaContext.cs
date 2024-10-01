using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PB303Pronia.Areas.Identity.Data;

namespace PB303Pronia.Data;

public class PB303ProniaContext : IdentityDbContext<PB303ProniaUser>
{
    public PB303ProniaContext(DbContextOptions<PB303ProniaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
