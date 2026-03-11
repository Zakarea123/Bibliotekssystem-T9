using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotekssystem_T9_App.Controllers;

public class LoansController : Controller
{
    private readonly LoanApiService  _loanApiService;

    public LoansController(LoanApiService loanApiService)
    {
        _loanApiService = loanApiService;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var borrowerId = 5; // TODO: replace with real user ID from Account API later
        var loans = await _loanApiService.GetBorrowerLoansAsync(borrowerId);
        return View(loans);
    }
}