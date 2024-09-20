using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PB303Pronia.Contexts;
using PB303Pronia.Models;

namespace PB303Pronia.Areas.admin.Controllers;
[Area("admin")]
public class SettingController : Controller
{

    private readonly AppDbContext _context;

    public SettingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {

        var settings=await _context.Settings.ToListAsync();
        return View(settings);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Setting setting)
    {
        if (!ModelState.IsValid)
            return View(setting);

        var isExist=await _context.Settings.AnyAsync(x=>x.Key==setting.Key);
        
        if (isExist)
        {
            ModelState.AddModelError("Key", "This key is already exist");
            return View(setting);
        }


        await _context.Settings.AddAsync(setting);
        await _context.SaveChangesAsync();  


        return RedirectToAction("Index");
    }
}
