using Bibliotekssystem_T9_App.Models;

namespace Bibliotekssystem_T9_App.Services;

public class CatalogApiService
{
    private readonly HttpClient _client;

    public CatalogApiService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("CatalogService");
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        var response = await _client.GetAsync("api/Items");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Item>>() ?? new();
    }

    public async Task<Item?> GetItemsAsync(int id)
    {
        var response = await _client.GetAsync($"api/Items/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Item>();
    }

    public async Task CreateItemAsync(Item item)
    {
        var response = await _client.PostAsJsonAsync("api/Items", item);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateItemAsync(int id, Item item)
    {
        var response = await _client.PutAsJsonAsync($"api/Items/{id}", item);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteItemAsync(int id)
    {
        var response = await _client.DeleteAsync($"api/Items/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ItemType>> GetItemTypesAsync()
    {
        var response = await _client.GetAsync("api/ItemTypes");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ItemType>>() ?? new();
    }
}