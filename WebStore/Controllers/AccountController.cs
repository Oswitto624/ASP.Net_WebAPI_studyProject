using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;

namespace WebStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _UserManager;
    private readonly SignInManager<User> _SignInManager;
    private readonly ILogger<AccountController> _Logger;

    public AccountController(UserManager<User> UserManager, 
                             SignInManager<User> SignInManager, 
                             ILogger<AccountController> Logger)
    {
        _UserManager = UserManager;
        _SignInManager = SignInManager;
        _Logger = Logger;
    }

    public IActionResult Register() => View(new RegisterUserViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if(!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            UserName = model.UserName,
        };

        var creation_result = await _UserManager.CreateAsync(user, model.Password);

        if (creation_result.Succeeded)
        {
            await _SignInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        foreach (var error in creation_result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl});

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
            return View(model);

        var login_result = await _SignInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            model.RememberMe,
            true);

        if (login_result.Succeeded)
        {
            //return Redirect("Index", "Home"); //не безопасно

            //if(Url.IsLocalUrl(model.ReturnUrl))
            //    return Redirect(model.ReturnUrl);
            //return RedirectToAction("Index", "Home");

            return LocalRedirect(model.ReturnUrl ?? "/");
        }
        //else if (login_result.RequiresTwoFactor)
        //{
        //    //выполнение двухфакторной проверки
        //}
        //else if (!login_result.IsLockedOut)
        //{
        //    //отправить пользователю информацию о том что он заблокирован
        //}

        ModelState.AddModelError("", "Неверное имя пользователя или пароль!");

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _SignInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");

    }

    public IActionResult AccessDenied() => View();

}
