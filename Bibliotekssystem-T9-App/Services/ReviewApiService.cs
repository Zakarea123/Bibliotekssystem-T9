using Bibliotekssystem_T9_App.Models;

namespace Bibliotekssystem_T9_App.Services;

public class ReviewApiService
{
    private readonly HttpClient _httpClient;

    public ReviewApiService(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("BookReviews");
    }

    public async Task<List<BookReviews>> GetReviewsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<BookReviews>>("/api/BookReviews");
        return response ?? new List<BookReviews>();
    }

    public async Task<BookReviews?> GetReviewAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<BookReviews>($"/api/BookReviews/{id}");
    }

    public async Task<bool> CreateReviewAsync(BookReviews review)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/BookReviews", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateReviewAsync(BookReviews review)
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