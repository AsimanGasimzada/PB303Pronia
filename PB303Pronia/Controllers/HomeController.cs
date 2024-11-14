using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PB303Pronia.Contexts;
using PB303Pronia.Repositories.Abstractions;
using PB303Pronia.Services.Abstactions;
using PB303Pronia.Services.Implementations;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Controllers;

public class HomeController : Controller
{

    private readonly AppDbContext _context;
    private readonly ILayoutService _layoutService;
    private const string COOKIE_BASKET_KEY = "basket";
    private readonly IEmailService _emailService;
    private readonly IProductRepository _productRepository;
    private readonly ISliderRepository _sliderRepository;

    public HomeController(AppDbContext context, ILayoutService layoutService, IEmailService emailService, IProductRepository productRepository, ISliderRepository sliderRepository)
    {
        _context = context;
        _layoutService = layoutService;
        _emailService = emailService;
        _productRepository = productRepository;
        _sliderRepository = sliderRepository;
    }

    public async Task<IActionResult> Index()
    {


        var products = await _productRepository.GetAll().ToListAsync();
        var sliders = await _sliderRepository.GetAll().ToListAsync();



        HomeViewModel model = new HomeViewModel
        {
            Products = products,
            Sliders = sliders
        };



        return View(model);
    }


    public async Task<IActionResult> AddToBasket(int id)
    {
        var product = await _context.Products.FindAsync(id);


        if (product is null)
            return NotFound();

        var basket = Request.Cookies[COOKIE_BASKET_KEY];

        List<BasketViewModel> basketViewModels = new List<BasketViewModel>();

        if (basket is { })
            basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket) ?? new();


        var isExist = basketViewModels.FirstOrDefault(x => x.ProductId == id);

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



        var json = JsonConvert.SerializeObject(basketViewModels);

        Response.Cookies.Append(COOKIE_BASKET_KEY, json);




        //var basketItems =  _layoutService.GetBasketAsync().Result;



        //return PartialView("_BasketPartial", basketItems);


        //return RedirectToAction("Redirect");


        var basketItems = await _layoutService.GetBasketAsync(basketViewModels);

        return PartialView("_BasketPartial", basketItems);





    }




    public async Task<IActionResult> Redirect()
    {

        var basketItems = await _layoutService.GetBasketAsync();

        return PartialView("_BasketPartial", basketItems);

    }


    public IActionResult SendEmail()
    {

        string body = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Forgot Password</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .email-container {\r\n            background-color: #ffffff;\r\n            max-width: 600px;\r\n            margin: 40px auto;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n        .email-header {\r\n            text-align: center;\r\n            border-bottom: 1px solid #dddddd;\r\n            padding-bottom: 20px;\r\n            margin-bottom: 20px;\r\n        }\r\n        .email-header h1 {\r\n            color: #333333;\r\n        }\r\n        .email-body {\r\n            text-align: center;\r\n        }\r\n        .email-body p {\r\n            font-size: 16px;\r\n            color: #555555;\r\n        }\r\n        .email-body a {\r\n            display: inline-block;\r\n            margin-top: 20px;\r\n            padding: 10px 20px;\r\n            font-size: 16px;\r\n            background-color: #007BFF;\r\n            color: #ffffff;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n        }\r\n        .email-footer {\r\n            margin-top: 30px;\r\n            font-size: 12px;\r\n            color: #888888;\r\n            text-align: center;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"email-container\">\r\n        <div class=\"email-header\">\r\n            <h1>Reset Your Password</h1>\r\n        </div>\r\n        <div class=\"email-body\">\r\n            <p>It looks like you requested to reset your password. Click the button below to create a new one:</p>\r\n            <a href=\"{{reset_link}}\">Reset Password</a>\r\n            <p>If you didn’t request this, please ignore this email.</p>\r\n        </div>\r\n        <div class=\"email-footer\">\r\n            <p>&copy; 2024 Your Company Name. All rights reserved.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";

        _emailService.SendEmail("agilma@code.edu.az", "Salam", body);

        return Ok();
    }
}
