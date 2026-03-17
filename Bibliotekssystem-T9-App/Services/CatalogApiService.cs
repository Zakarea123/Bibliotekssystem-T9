using Bibliotekssystem_T9_App.Models;

namespace Bibliotekssystem_T9_App.Services;

public class CatalogApiService
{
    private readonly HttpClient _client;
    private readonly string _apiKey;

    public CatalogApiService(IHttpClientFactory clientFactory, IConfiguration config)
    {
        _client = clientFactory.CreateClient("CatalogService");
        _apiKey = config["CatalogService:ApiKey"] ?? "";
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        var response = await _client.GetAsync("api/Items");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Item>>() ?? new();
    }

    public async Task<Item?> GetItemAsync(int id)
    {
        var response = await _client.GetAsync($"api/Items/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Item>();
    }

    public async Task CreateItemAsync(Item item)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/Items");
        request.Headers.Add("X-Api-Key", _apiKey);
        request.Content = JsonContent.Create(item);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateItemAsync(int id, Item item)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/Items/{id}");
        request.Headers.Add("X-Api-Key", _apiKey);
        request.Content = JsonContent.Create(item);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteItemAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Items/{id}");
        request.Headers.Add("X-Api-Key", _apiKey);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ItemType>> GetItemTypesAsync()
    {
        var response = await _client.GetAsync("api/ItemTypes");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ItemType>>() ?? new();
    }
}