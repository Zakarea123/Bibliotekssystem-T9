using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Bibliotekssystem_T9_App.Models;
using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authorization;

namespace Bibliotekssystem_T9_App.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LoanApiService _loanApiService;

    public HomeController(ILogger<HomeController> logger,  LoanApiService loanApiService)
    {
        _logger = logger;
        _loanApiService = loanApiService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Admin"))
        {
            var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var loans = await _loanApiService.GetBorrowerLoansAsync(borrowerId);

            ViewBag.ActiveLoans = loans.Count;
            ViewBag.NextReturn = loans
                .OrderBy(l => l.DueDate)
                .FirstOrDefault()?.DueDate.ToString("d MMM") ?? "—";
        }
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
