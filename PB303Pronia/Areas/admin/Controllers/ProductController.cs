using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB303Pronia.Contexts;
using PB303Pronia.Helpers;
using PB303Pronia.Models;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Areas.admin.Controllers;
[Area("Admin")]
[AutoValidateAntiforgeryToken]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string FOLDER_PATH = "";

    public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;

        FOLDER_PATH = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images");
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Include(p => p.ProductImages).ToListAsync();

        return View(products);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        #region FileValidations
        if (!vm.MainImage.CheckType())
        {
            ModelState.AddModelError("MainImage", "Please enter valid input");
            return View(vm);
        }

        if (!vm.MainImage.CheckSize(2))
        {
            ModelState.AddModelError("MainImage", "Please enter valid input");
            return View(vm);
        }


        if (!vm.HoverImage.CheckType())
        {
            ModelState.AddModelError("HoverImage", "Please enter valid input");
            return View(vm);
        }


        if (!vm.HoverImage.CheckSize(2))
        {
            ModelState.AddModelError("HoverImage", "Please enter valid input");
            return View(vm);
        }



        foreach (var image in vm.AdditionalImages)
        {
            if (!image.CheckType())
            {
                ModelState.AddModelError("AdditionalImages", "Please enter valid input");
                return View(vm);
            }

            if (!image.CheckSize(2))
            {
                ModelState.AddModelError("AdditionalImages", "Please enter valid input");
                return View(vm);
            }
        }



        #endregion


        Product product = new()
        {
            Name = vm.Name,
            Price = vm.Price,
            Rating = vm.Rating,
        };



        #region CreateFiles
        string mainImagePath = await vm.MainImage.CreateImageAsync(FOLDER_PATH);

        ProductImage mainImage = new()
        {
            IsMain = true,
            Path = mainImagePath,
            Product = product
        };

        product.ProductImages.Add(mainImage);


        string hoverImagePath = await vm.HoverImage.CreateImageAsync(FOLDER_PATH);


        ProductImage hoverImage = new()
        {
            IsHover = true,
            Path = hoverImagePath,
            Product = product
        };

        product.ProductImages.Add(hoverImage);

        foreach (var image in vm.AdditionalImages)
        {
            string imagePath = await image.CreateImageAsync(FOLDER_PATH);

            ProductImage productImg = new()
            {
                Path = imagePath,
                Product = product
            };

            product.ProductImages.Add(productImg);

        }
        #endregion

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();


        ProductUpdateViewModel vm = new()
        {
            Id = id,
            Name = product.Name,
            Price = product.Price,
            Rating = product.Rating,
            HoverImagePath = product.ProductImages.FirstOrDefault(x => x.IsHover)?.Path ?? "undifenied",
            MainImagePath = product.ProductImages.FirstOrDefault(x => x.IsMain)?.Path ?? "undifenied",
            AdditionalImagePaths = product.ProductImages.Where(x => x.IsMain == false && x.IsHover == false).Select(x => x.Path).ToList(),
            AdditionalImageIds = product.ProductImages.Where(x => x.IsMain == false && x.IsHover == false).Select(x => x.Id).ToList()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateViewModel vm)
    {

        if (!ModelState.IsValid)
            return View(vm);


        var existProduct = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existProduct is null)
            return BadRequest();



        #region File Validations
        if (!vm.MainImage?.CheckType() ?? false)
        {
            ModelState.AddModelError("MainImage", "Please enter valid input");
            return View(vm);
        }

        if (!vm.MainImage?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("MainImage", "Please enter valid input");
            return View(vm);
        }

        if (!vm.HoverImage?.CheckType() ?? false)
        {
            ModelState.AddModelError("HoverImage", "Please enter valid input");
            return View(vm);
        }

        if (!vm.HoverImage?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("HoverImage", "Please enter valid input");
            return View(vm);
        }



        foreach (var image in vm.AdditionalImages)
        {
            if (!image.CheckType())
            {
                ModelState.AddModelError("AdditionalImages", "Please enter valid input");
                return View(vm);
            }

            if (!image.CheckSize(2))
            {
                ModelState.AddModelError("AdditionalImages", "Please enter valid input");
                return View(vm);
            }
        }
        #endregion

        #region modifie MainImage
        if (vm.MainImage is { })
        {
            var mainImage = existProduct.ProductImages.FirstOrDefault(x => x.IsMain);

            string mainImagePath = await vm.MainImage.CreateImageAsync(FOLDER_PATH);
            if (mainImage != null)
            {
                mainImage.Path.DeleteFile(FOLDER_PATH);
                mainImage!.Path = mainImagePath;

                _context.ProductImages.Update(mainImage);
            }
            else
            {
                ProductImage newMainImage = new()
                {
                    IsMain = true,
                    Path = mainImagePath,
                    Product = existProduct
                };

                existProduct.ProductImages.Add(newMainImage);
            }
        }
        #endregion

        #region modifie HoverImage
        if (vm.HoverImage is { })
        {
            var hoverImage = existProduct.ProductImages.FirstOrDefault(x => x.IsHover);
            string hoverImagePath = await vm.HoverImage.CreateImageAsync(FOLDER_PATH);

            if (hoverImage is not null)
            {
                hoverImage.Path.DeleteFile(FOLDER_PATH);
                hoverImage.Path = hoverImagePath;

                _context.ProductImages.Update(hoverImage);
            }
            else
            {
                ProductImage newHoverImage = new()
                {
                    IsHover = true,
                    Path = hoverImagePath,
                    Product = existProduct
                };

                existProduct.ProductImages.Add(newHoverImage);
            }
        }

        #endregion

        #region delete old images


        var oldImgIds = existProduct.ProductImages.Where(x => x.IsMain == false && x.IsHover == false).Select(x => x.Id).ToList();


        foreach (var imageId in oldImgIds)
        {

            var isExist = vm.AdditionalImageIds.Any(x => x == imageId);

            if (isExist is false)
            {
                var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);

                if (image is { })
                {
                    image.Path.DeleteFile(FOLDER_PATH);
                    _context.ProductImages.Remove(image);
                }
            }

        }

        #endregion

        #region create new images
        foreach (var image in vm.AdditionalImages)
        {
            string imagePath = await image.CreateImageAsync(FOLDER_PATH);

            ProductImage productImage = new()
            {
                Path = imagePath,
                Product = existProduct
            };
            existProduct.ProductImages.Add(productImage);

        }
        #endregion

        existProduct.Name = vm.Name;
        existProduct.Rating = vm.Rating;
        existProduct.Price = vm.Price;

        
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }
}
