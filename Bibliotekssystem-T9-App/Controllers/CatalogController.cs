using Bibliotekssystem_T9_App.Models;
using Bibliotekssystem_T9_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotekssystem_T9_App.Controllers;

public class CatalogController : Controller
{
    private readonly CatalogApiService _catalogApiService;

    public CatalogController(CatalogApiService catalogApiService)
    {
        _catalogApiService = catalogApiService;
    }

    //GET: Visar alla objekt i katalogen
    public async Task<IActionResult> Index()
    {
        var items = await _catalogApiService.GetItemsAsync();
        return View(items);
    }

    //GET: Visar detaljer för ett specifikt objekt
    public async Task<IActionResult> Details(int id)
    {
        var item = await _catalogApiService.GetItemsAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }
    
    //GET: Visar formulär för att skapa nytt objekt
    [Authorize]
    public async Task<IActionResult> Create()
    {
        ViewBag.ItemTypes = await _catalogApiService.GetItemTypesAsync();
        return View();
    }

    //POST: Skickar nytt objekt till API:t
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        await _catalogApiService.CreateItemAsync(item);
        return RedirectToAction(nameof(Index));
    }

    //GET: Visar formulär för att redigera ett objekt
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _catalogApiService.GetItemsAsync(id);
        if (item is null) return NotFound();
        ViewBag.ItemTypes = await _catalogApiService.GetItemTypesAsync();
        return View(item);
    }

    //POST: Skickar uppdaterat objekt till API:t
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Item item)
    {
        await _catalogApiService.UpdateItemAsync(id, item);
        return RedirectToAction(nameof(Index));
    }

    //GET: Visar bekräftelsesida för borttagning
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _catalogApiService.GetItemsAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _catalogApiService.DeleteItemAsync(id);
        return RedirectToAction(nameof(Index));
    }
    
}