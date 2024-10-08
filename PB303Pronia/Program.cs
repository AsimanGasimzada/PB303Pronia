using Microsoft.EntityFrameworkCore;
using PB303Pronia.Contexts;
using PB303Pronia.Repositories.Abstractions;
using PB303Pronia.Repositories.Implementations;
using PB303Pronia.Services.Abstactions;
using PB303Pronia.Services.Implementations;

namespace PB303Pronia;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();


        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ILayoutService, LayoutService>();
        builder.Services.AddScoped<IEmailService, EmailService>();


        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ISliderRepository,SliderRepository>();

        var app = builder.Build();


        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication(); ;
        app.UseAuthorization();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}