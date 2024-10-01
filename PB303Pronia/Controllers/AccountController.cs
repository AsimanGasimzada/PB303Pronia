using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PB303Pronia.Models;
using PB303Pronia.ViewModels;

namespace PB303Pronia.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if(!ModelState.IsValid)
            return View(vm);

        var user=await _userManager.FindByEmailAsync(vm.Email);

        if(user is null)
        {
            ModelState.AddModelError("", "Sifre ve ya password yanlisdir");
            return View(vm);    
        }


        var result = await _signInManager.PasswordSignInAsync(user, vm.Password,true,true);

        if(result.Succeeded is false)
        {
            ModelState.AddModelError("", "Sifre ve ya password yanlisdir");
            return View(vm);
        }

        if (vm.ReturnUrl is not null)
            return Redirect(vm.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        AppUser user = new()
        {
            Email = vm.Email,
            UserName = vm.Username,

        };

        var result = await _userManager.CreateAsync(user, vm.Password);


        if (!result.Succeeded)
        {


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(vm);
        }


        await _signInManager.SignInAsync(user, isPersistent: false);

        return RedirectToAction("Index","Home");

    }
}
