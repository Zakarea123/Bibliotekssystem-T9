using Microsoft.AspNetCore.Mvc;
using Bibliotekssystem_T9_App.Services;
using Bibliotekssystem_T9_App.Models;
using System.Security.Claims;

namespace Bibliotekssystem_T9_App.Controllers;

public class ReviewsController : Controller
{
    private readonly ReviewApiService _reviewService;
    private readonly NotificationApiService _notificationApiService;

    public ReviewsController(ReviewApiService reviewService, NotificationApiService notificationApiService)
    {
        _reviewService = reviewService;
        _notificationApiService = notificationApiService;
    }

    // LISTA
    public async Task<IActionResult> Index(string searchTerm)
    {
        var reviews = await _reviewService.GetReviewsAsync();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();

            reviews = reviews.Where(r =>
                (r.BookTitle ?? "").ToLower().Contains(searchTerm) ||
                (r.ReviewerName ?? "").ToLower().Contains(searchTerm) ||
                (r.Text ?? "").ToLower().Contains(searchTerm)
            ).ToList();
        }

        return View(reviews);
    }

    // Create GET
    public IActionResult Create()
    {
        return View();
    }

    // Create POST
    [HttpPost]
    public async Task<IActionResult> Create(BookReviews review)
    {
        review.ReviewerName = User.Identity!.Name;
        
        await _reviewService.CreateReviewAsync(review);
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _notificationApiService.CreateNotificationAsync(adminId, 8);
        return RedirectToAction("Index");
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(int id)
    {
        var review = await _reviewService.GetReviewAsync(id);

        if (review == null)
            return NotFound();

        return View(review);
    }

    // EDIT (POST)
    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookReviews review)
    {
        if (!User.IsInRole("Admin") && review.ReviewerName != User.Identity!.Name)
            return Forbid();

        await _reviewService.UpdateReviewAsync(review);
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _notificationApiService.CreateNotificationAsync(adminId, 8);

        return RedirectToAction("Index");
    }

    // DELETE (GET)
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _reviewService.GetReviewAsync(id);
        if (review == null) return NotFound();

        return View(review);
    }

    // DELETE (POST)
    [HttpPost]
    public async Task<IActionResult> Delete(int id, BookReviews review)
    {
        if (!User.IsInRole("Admin") && review.ReviewerName != User.Identity!.Name)
            return Forbid();
        
        await _reviewService.DeleteReviewAsync(id);
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _notificationApiService.CreateNotificationAsync(adminId, 8);
        return RedirectToAction("Index");
    }
}