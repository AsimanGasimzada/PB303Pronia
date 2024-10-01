using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB303Pronia.Contexts;
using PB303Pronia.Helpers;
using PB303Pronia.Models;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Areas.admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
public class SliderController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string FOLDER_PATH = "";

    public SliderController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;

        FOLDER_PATH = Path.Combine(_environment.WebRootPath, "assets", "images");
    }


    public async Task<IActionResult> Index()
    {
        var sliders = await _context.Sliders.ToListAsync();

        return View(sliders);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(SliderCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);



        if (!vm.Image.CheckType())
        {
            ModelState.AddModelError("Image", "Please enter valid input");
            return View(vm);
        }
        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Please enter valid input");
            return View(vm);
        }



        string imagePath = await vm.Image.CreateImageAsync(FOLDER_PATH);



        Slider slider = new Slider()
        {
            Description = vm.Description,
            ImageUrl = imagePath,
            SubTitle = vm.Subtitle,
            Title = vm.Title,
        };

        await _context.Sliders.AddAsync(slider);

        await _context.SaveChangesAsync();  



        return RedirectToAction("Index");

    }


}
