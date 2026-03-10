using Microsoft.AspNetCore.Mvc;

namespace Bibliotekssystem_T9_App.Controllers;

public class LoansController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}