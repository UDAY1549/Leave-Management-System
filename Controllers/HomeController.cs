using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller
{
    [Authorize]
    public IActionResult Secure()
    {
        return View(); // Create Views/Home/Secure.cshtml to test visually
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Privacy()
    {
    return View();
    }

}