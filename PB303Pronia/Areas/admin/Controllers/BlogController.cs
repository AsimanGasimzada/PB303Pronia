using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PB303Pronia.Contexts;
using PB303Pronia.Helpers;
using PB303Pronia.Models;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Areas.admin.Controllers;
[Area("Admin")]
public class BlogController : Controller
{

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string FOLDER_PATH;

    public BlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        FOLDER_PATH = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images");
    }
    public async Task<IActionResult> Index()
    {
        var blogs = await _context.Blogs.ToListAsync();
        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {
        BlogCreateViewModel vm = new();

        vm = await IncludeCategories(vm);

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateViewModel vm)
    {
        vm = await IncludeCategories(vm);


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


        foreach (var categoryId in vm.CategoryIds)
        {
            var isExist = await _context.Categories.AnyAsync(x => x.Id == categoryId);

            if (!isExist)
            {
                ModelState.AddModelError("CategoryIds", "Category not found");
                return View(vm);
            }
        }



        string imagePath = await vm.Image.CreateImageAsync(FOLDER_PATH);
        Blog blog = new()
        {
            Author = vm.Author,
            Description = vm.Description,
            Title = vm.Title,
            ImagePath = imagePath,
        };


        foreach (var categoryId in vm.CategoryIds)
        {
            BlogCategory blogCategory = new()
            {
                CategoryId = categoryId,
                Blog = blog,
            };

            blog.BlogCategories.Add(blogCategory);
        }


        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Update(int id)
    {
        var blog = await _context.Blogs.Include(x => x.BlogCategories).FirstOrDefaultAsync(x => x.Id == id);

        if (blog is null)
            return NotFound();

        BlogUpdateViewModel vm = new()
        {
            Author = blog.Author,
            Title = blog.Title,
            Description = blog.Description,
            Id = id,
            CategoryIds = blog.BlogCategories.Select(x => x.CategoryId).ToList(),

        };

        vm = await IncludeCategories(vm);

        return View(vm);
    }



    private async Task<BlogCreateViewModel> IncludeCategories(BlogCreateViewModel vm)
    {
        var categories = await _context.Categories.ToListAsync();



        foreach (var category in categories)
        {
            vm.CategoryList.Add(new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
        }

        return vm;
    }

    private async Task<BlogUpdateViewModel> IncludeCategories(BlogUpdateViewModel vm)
    {
        var categories = await _context.Categories.ToListAsync();



        foreach (var category in categories)
        {
            vm.CategoryList.Add(new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
        }

        return vm;
    }

    [HttpPost]
    public async Task<IActionResult> Update(BlogUpdateViewModel vm)
    {
        vm = await IncludeCategories(vm);

        if (!ModelState.IsValid)
            return View(vm);


        var existBlog = await _context.Blogs.Include(x => x.BlogCategories).FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existBlog is null)
            return BadRequest();

        if (!vm.Image?.CheckType() ?? false)
        {
            ModelState.AddModelError("Image", "Please enter valid input");
            return View(vm);
        }
        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Please enter valid input");
            return View(vm);
        }


        foreach (var categoryId in vm.CategoryIds)
        {
            var isExist = await _context.Categories.AnyAsync(x => x.Id == categoryId);

            if (!isExist)
            {
                ModelState.AddModelError("CategoryIds", "Category not found");
                return View(vm);
            }
        }



        existBlog.Author = vm.Author;
        existBlog.Description = vm.Description;
        existBlog.Title = vm.Title;


        if (vm.Image is { })
            existBlog.ImagePath = await vm.Image.CreateImageAsync(FOLDER_PATH);

        existBlog.BlogCategories = new List<BlogCategory>();




        foreach (var categoryId in vm.CategoryIds)
        {
            BlogCategory blogCategory = new()
            {
                CategoryId = categoryId,
                BlogId = existBlog.Id,
            };

            existBlog.BlogCategories.Add(blogCategory);
        }


         _context.Blogs.Update(existBlog);
        await _context.SaveChangesAsync();



        return RedirectToAction("Index");




    }
}
