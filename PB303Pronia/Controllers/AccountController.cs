using Microsoft.AspNetCore.Mvc;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(LoginVM vm)
        {


            if(vm.ReturnUrl is not null)
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index","Home");
        }
    }
}
