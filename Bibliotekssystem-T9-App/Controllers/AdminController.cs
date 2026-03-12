using Bibliotekssystem_T9_App.Dtos;
using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotekssystem_T9_App.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly UserApiService _userApiService;

    public AdminController(UserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userApiService.GetUsersAsync();

        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _userApiService.DeleteUserAsync(id);
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

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var users = await _userApiService.GetUsersAsync();
        var user = users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            return NotFound();

        return View(user);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(UserDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var success = await _userApiService.UpdateUserAsync(dto);

        if (!success)
        {
            ModelState.AddModelError("", "Kunde inte uppdatera användaren.");
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }
}