using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PB303Pronia.Contexts;
using PB303Pronia.Services.Abstactions;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Services.Implementations
{
    public class LayoutService : ILayoutService
    {

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private const string COOKIE_BASKET_KEY = "basket";
        public LayoutService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            var settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);

            return settings;
        }


        public async  Task<List<BasketProductViewModel>> GetBasketAsync()
        {
            string? basket = _contextAccessor.HttpContext?.Request.Cookies[COOKIE_BASKET_KEY];

            List<BasketViewModel> basketViewModels= new List<BasketViewModel>();


            if(basket is { })
                basketViewModels=JsonConvert.DeserializeObject<List<BasketViewModel>>(basket) ?? new();



            List<BasketProductViewModel> basketItems = new();


            foreach (var item in basketViewModels)
            {
                var product =await _context.Products.FindAsync(item.ProductId);


                if(product is {})
                {
                    BasketProductViewModel vm = new()
                    {
                        Product=product,
                        Count=item.Count,
                    };

                    basketItems.Add(vm);
                }
            }


            return basketItems;
        }
    }
}
