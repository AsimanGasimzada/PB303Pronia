using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PB303Pronia.Contexts;
using PB303Pronia.Services.Abstactions;
using PB303Pronia.Services.Implementations;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Controllers;

public class HomeController : Controller
{

    private readonly AppDbContext _context;
    private readonly ILayoutService _layoutService;
    private const string COOKIE_BASKET_KEY = "basket";

    public HomeController(AppDbContext context, ILayoutService layoutService)
    {
        _context = context;
        _layoutService = layoutService;
    }

    public async Task<IActionResult> Index()
    {


        var products=await _context.Products.ToListAsync();
        

        HomeViewModel model = new HomeViewModel();


        model.Products = products;
        
        return View(model);
    }


    public async Task<IActionResult> AddToBasket(int id)
    {
        var product=await _context.Products.FindAsync(id);


        if(product is null)
            return NotFound();

        var basket = Request.Cookies[COOKIE_BASKET_KEY];

        List<BasketViewModel> basketViewModels = new List<BasketViewModel>();

        if (basket is { })
            basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket) ?? new();


        var isExist=basketViewModels.FirstOrDefault(x=>x.ProductId==id);

        if (isExist is { })
            isExist.Count++;
        else
        {
            BasketViewModel vm = new()
            {
                ProductId = id,
                Count = 1,
            };

            basketViewModels.Add(vm);
        }



        var json=JsonConvert.SerializeObject(basketViewModels);

        Response.Cookies.Append(COOKIE_BASKET_KEY, json);




        //var basketItems =  _layoutService.GetBasketAsync().Result;



        //return PartialView("_BasketPartial", basketItems);


        return RedirectToAction("Redirect");


    }




  public async Task<IActionResult> Redirect()
    {

        var basketItems=await _layoutService.GetBasketAsync();

        return PartialView("_BasketPartial", basketItems);

    }
}
