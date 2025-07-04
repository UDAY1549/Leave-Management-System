using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        // return view or redirect to identity login
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
}