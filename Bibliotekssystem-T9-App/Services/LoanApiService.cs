using Bibliotekssystem_T9_App.Models;

namespace Bibliotekssystem_T9_App.Services;

public class LoanApiService
{
    private HttpClient _client;

    public LoanApiService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("LoanService");
    }

    // Calls GET {{/api/loans/borrower/{borrowerId}}} and deserializes the response into a list of loans.
    public async Task<List<Loan>> GetBorrowerLoansAsync(int borrowerId)
    {
        var url = $"api/loans/borrower/{borrowerId}";
        Console.WriteLine($"Calling: {_client.BaseAddress}{url}");
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Loan>>() ?? new List<Loan>();
    }
    
    // Calls GET {{/api/loans/borrower/{borrowerId}/history}} and deserializes the response into a list of loans.
    public async Task<List<Loan>> GetBorrowerHistoryAsync(int borrowerId)
    {
        var response = await _client.GetAsync($"api/loans/borrower/{borrowerId}/history");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Loan>>() ?? new List<Loan>();
    }
    
    // Calls PUT {{/api/loans/{id}/return}} to mark a loan as returned.
    public async Task ReturnLoanAsync(int loanId)
    {
        var response = await _client.PutAsync($"api/loans/{loanId}/return", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Loan?> GetLoanAsync(int loanId)
    {
        var response = await _client.GetAsync($"api/loans/{loanId}");
        if(!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<Loan>();
    }
    
    // Calls GET {{/api/loans/active}} and returns all active loans
    public async Task<List<Loan>> GetActiveLoansAsync()
    {
        var response = await _client.GetAsync("api/loans/active");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Loan>>() ?? new List<Loan>();
    }
    
    // Calls POST /api/loans to create a new loan
    public async Task CreateLoanAsync(int itemId, int borrowerId, DateTime dueDate)
    {
        var response = await _client.PostAsJsonAsync("api/loans", new { itemId, borrowerId, dueDate });
        response.EnsureSuccessStatusCode();
    }
}