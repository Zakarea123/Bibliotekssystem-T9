using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotekssystem_T9_App.Controllers;

[Authorize] // Kräver inloggning. Man kan ej nå notiser utan att vara autentiserad.
public class NotificationsController : Controller
{
    // Konstruktorn
    private readonly NotificationApiService _notificationService;

    public NotificationsController(NotificationApiService notificationService)
    {
        _notificationService = notificationService;
    }
    
    // Metoden som körs när fetch i JS körs så hämtar metoden notiserna och returnerar PartialView.
    public async Task<IActionResult> Dropdown() 
    {
        //Här ska det riktiga userId från claims när userService är klart
        var userId = 1;
        
        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        return PartialView("_NotificationDropdown", notifications);
    }

    // Metod för att anropa PUT och uppdatera notiserna som lästa eller olästa.
    [HttpPost]
    public async Task<IActionResult> MarkRead(int id)
    {
        await _notificationService.UpdateReadStatusAsync(id, true);
        return Ok();
    }
    
    // Metod för att anropa DELETE och Ok() betyder att det gick igenom. 
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _notificationService.DeleteNotificationAsync(id);
        return Ok();
    }
}