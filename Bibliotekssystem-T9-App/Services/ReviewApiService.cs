using Bibliotekssystem_T9_App.Models;

namespace Bibliotekssystem_T9_App.Services;

public class ReviewApiService
{
    private readonly HttpClient _httpClient;

    public ReviewApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BookReview>> GetReviewsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<BookReview>>("/api/BookReviews");
        return response ?? new List<BookReview>();
    }

    public async Task<BookReview?> GetReviewAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<BookReview>($"/api/BookReviews/{id}");
    }

    public async Task<bool> CreateReviewAsync(BookReview review)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/BookReviews", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateReviewAsync(BookReview review)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/BookReviews/{review.ReviewId}", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteReviewAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/BookReviews/{id}");
        return response.IsSuccessStatusCode;
    }
}