using System.Security.Claims;
using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotekssystem_T9_App.Controllers;

public class LoansController : Controller
{
    private readonly LoanApiService  _loanApiService;
    private readonly CatalogApiService _catalogApiService;
    private readonly NotificationApiService _notificationApiService;

    public LoansController(LoanApiService loanApiService , CatalogApiService catalogApiService, NotificationApiService notificationApiService)
    {
        _loanApiService = loanApiService;
        _catalogApiService = catalogApiService;
        _notificationApiService = notificationApiService;
    }
    
    // GET: Fetches active loans for the current user from LoanService
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        var loans = await _loanApiService.GetBorrowerLoansAsync(borrowerId); 
        
        // Fetch item titles for each loan and store in a dictionary
        var itemTitles = new Dictionary<int, string>();
        foreach (var loan in loans)
        { 
            var item = await _catalogApiService.GetItemAsync(loan.ItemId); 
            itemTitles[loan.ItemId] = item?.Title ?? $"Objekt {loan.ItemId}";
        }

        ViewBag.ItemTitles = itemTitles;
        return View(loans);
    }
    
    // Fetches full loan history for the current user from LoanService and passes it to the view.
    [Authorize]
    public async Task<IActionResult> History()
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        var history = await _loanApiService.GetBorrowerHistoryAsync(borrowerId);
        var itemTitles = new Dictionary<int, string>();
        
        foreach (var loan in history)
        { 
            var item = await _catalogApiService.GetItemAsync(loan.ItemId); 
            itemTitles[loan.ItemId] = item?.Title ?? $"Objekt {loan.ItemId}";
        }

        ViewBag.ItemTitles = itemTitles;
        return View(history);
    }
    
    // Shows active loans with checkboxes for returning
    [Authorize]
    public async Task<IActionResult> Return()
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var loans = await _loanApiService.GetBorrowerLoansAsync(borrowerId);
        
        var itemTitles = new Dictionary<int, string>();
        foreach (var loan in loans)
        { 
            var item = await _catalogApiService.GetItemAsync(loan.ItemId); 
            itemTitles[loan.ItemId] = item?.Title ?? $"Objekt {loan.ItemId}";
        }

        ViewBag.ItemTitles = itemTitles;
        return View(loans);
    }
    
    // Handles the return form submission
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ReturnSelected(List<int> selectedLoanIds)
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        foreach (var loanId in selectedLoanIds)
        {
            await _loanApiService.ReturnLoanAsync(loanId);
        }
        await _notificationApiService.CreateNotificationAsync(borrowerId, 5);
        TempData["SuccessMessage"] = $"{selectedLoanIds.Count} objekt har returnerats!";
        return RedirectToAction("Index");
    }
    
    [Authorize]
    public async Task<IActionResult> Borrow()
    {
        var items = await _catalogApiService.GetItemsAsync();
        var activeLoans = await _loanApiService.GetActiveLoansAsync();

        // Build a HashSet of borrowed ItemIds ( Faster than list and esaier to track)
        var borrowedItemIds = activeLoans.Select(l => l.ItemId).ToHashSet();
        ViewBag.BorrowedItemIds = borrowedItemIds;
        return View(items);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateLoan(int itemId, DateTime dueDate)
    {
        var borrowerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _loanApiService.CreateLoanAsync(itemId, borrowerId, dueDate);
        await _notificationApiService.CreateNotificationAsync(borrowerId, 4, dueDate);

        TempData["SuccessMessage"] = "Lånet har registrerats!";
        return RedirectToAction("Borrow");
    }

    
    
}