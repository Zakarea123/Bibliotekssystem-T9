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
    public async Task<IActionResult> Index()
    {
        var reviews = await _reviewService.GetReviewsAsync();
        return View(reviews);
    }

    // CREATE (GET)
    public IActionResult Create()
    {
        return View();
    }

    // CREATE (POST)
    [HttpPost]
    public async Task<IActionResult> Create(BookReview review)
    {
        await _reviewService.CreateReviewAsync(review);
        return RedirectToAction("Index");
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(int id)
    {
        var review = await _reviewService.GetReviewAsync(id);
        if (review == null) return NotFound();

        return View(review);
    }

    // EDIT (POST)
    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookReview review)
    {
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
    public async Task<IActionResult> Delete(int id, BookReview review)
    {
        await _reviewService.DeleteReviewAsync(id);
        return RedirectToAction("Index");
    }
}