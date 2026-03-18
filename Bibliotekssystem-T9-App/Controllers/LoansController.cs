using System.Security.Claims;
using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotekssystem_T9_App.Controllers;

public class LoansController : Controller
{
    private readonly LoanApiService  _loanApiService;

    public LoansController(LoanApiService loanApiService)
    {
        _loanApiService = loanApiService;
    }
    
    // GET: Fetches active loans for the current user from LoanService
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        var loans = await _loanApiService.GetBorrowerLoansAsync(borrowerId);
        return View(loans);
    }
    
    // Fetches full loan history for the current user from LoanService and passes it to the view.
    [Authorize]
    public async Task<IActionResult> History()
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        var history = await _loanApiService.GetBorrowerHistoryAsync(borrowerId);
        return View(history);
    }
    
    // Shows active loans with checkboxes for returning
    [Authorize]
    public async Task<IActionResult> Return()
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var loans = await _loanApiService.GetBorrowerLoansAsync(borrowerId);
        return View(loans);
    }
    
    // Handles the return form submission
    [HttpPost]
    public async Task<IActionResult> ReturnSelected(List<int> selectedLoanIds)
    {
        foreach (var loanId in selectedLoanIds)
        {
            await _loanApiService.ReturnLoanAsync(loanId);
        }
        return RedirectToAction("Index");
    }

    
}