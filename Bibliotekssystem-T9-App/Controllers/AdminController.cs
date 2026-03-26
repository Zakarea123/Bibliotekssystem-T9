using Bibliotekssystem_T9_App.Dtos;
using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bibliotekssystem_T9_App.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserApiService _userApiService;
    private readonly NotificationApiService _notificationApiService;
    private readonly LoanApiService _loanApiService;
    private readonly CatalogApiService _catalogApiService; 
    public AdminController(UserApiService userApiService, NotificationApiService notificationApiService,
        LoanApiService loanApiService, CatalogApiService catalogApiService)
    {
        _userApiService = userApiService;
        _notificationApiService = notificationApiService;
        _loanApiService = loanApiService;
        _catalogApiService = catalogApiService;
    }

    public async Task<IActionResult> Index(string? searchTerm)
    {
        var users = await _userApiService.GetUsersAsync();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            users = users
                .Where(u =>
                    u.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Role.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Id.ToString().Contains(searchTerm))
                .ToList();
        }

        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _userApiService.DeleteUserAsync(id);
        
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _notificationApiService.CreateNotificationAsync(adminId, 7);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var success = await _userApiService.CreateUserAsync(dto);

        if (!success)
        {
            ModelState.AddModelError("", "Kunde inte skapa användaren.");
            return View(dto);
        }
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _notificationApiService.CreateNotificationAsync(adminId, 7);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var users = await _userApiService.GetUsersAsync();
        var user = users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            return NotFound();

        var dto = new EditUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Password = string.Empty
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var success = await _userApiService.UpdateUserAsync(dto);

        if (!success)
        {
            ModelState.AddModelError("", "Kunde inte uppdatera användaren.");
            return View(dto);
        }
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _notificationApiService.CreateNotificationAsync(adminId, 7);
        
        return RedirectToAction(nameof(Index));
    }
}