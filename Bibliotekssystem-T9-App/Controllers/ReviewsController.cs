using Microsoft.AspNetCore.Mvc;
using Bibliotekssystem_T9_App.Services;
using Bibliotekssystem_T9_App.Models;

namespace Bibliotekssystem_T9_App.Controllers;

public class ReviewsController : Controller
{
    private readonly ReviewApiService _reviewService;

    public ReviewsController(ReviewApiService reviewService)
    {
        _reviewService = reviewService;
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
        await _reviewService.CreateReviewAsync(review);
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
        if (id != review.ReviewId)
            return BadRequest();

        await _reviewService.UpdateReviewAsync(review);

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
        await _reviewService.DeleteReviewAsync(id);
        return RedirectToAction("Index");
    }
}