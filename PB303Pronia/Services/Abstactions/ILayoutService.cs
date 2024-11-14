using PB303Pronia.ViewModels;

namespace PB303Pronia.Services.Abstactions;

public interface ILayoutService
{
    Task<Dictionary<string, string>> GetSettingsAsync();
    Task<List<BasketProductViewModel>> GetBasketAsync();
    Task<List<BasketProductViewModel>> GetBasketAsync(List<BasketViewModel> basketItems);

}
