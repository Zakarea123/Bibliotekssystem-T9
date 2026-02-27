using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bibliotekssystem_T9_App.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        // ===== DEMO-KONTON =====
        var isAdmin =
            email.Equals("admin@library.local", StringComparison.OrdinalIgnoreCase) &&
            password == "Admin123!";

        var isEmployee =
            email.Equals("employee@organization.com", StringComparison.OrdinalIgnoreCase) &&
            password == "Password123!";

        if (!isAdmin && !isEmployee)
        {
            ViewBag.Error = "Fel e-post eller lösenord.";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        var role = isAdmin ? "Admin" : "Employee";

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, email),
            new(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        // Skicka tillbaka användaren dit den kom ifrån
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        // Annars skicka beroende på roll
        return role == "Admin"
            ? RedirectToAction("Index", "Admin")
            : RedirectToAction("Index", "Library");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}