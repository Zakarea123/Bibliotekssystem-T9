using Bibliotekssystem_T9_App.Models;

namespace Bibliotekssystem_T9_App.Services;

public class NotificationApiService
{
  private readonly HttpClient _client; //HTTP-klient som pratar med notification-API:et
  private readonly string _apiKey; //Nyckeln är sparad i user-secrets

  public NotificationApiService(IHttpClientFactory clientFactory, IConfiguration config)
  {
    _client = clientFactory.CreateClient("NotificationService");
    _apiKey = config["NotificationService:ApiKey"] ?? string.Empty;
  }

  public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
  {
    var response = await _client.GetAsync($"api/notifications/user/{userId}"); // skickar ett HTTP GET till API:et.
    response.EnsureSuccessStatusCode(); // Undantag om API:et svarar med felkod
    return await response.Content.ReadFromJsonAsync<List<Notification>>() ?? // JSON-svaret till en lista av Notification-objekt
           new List<Notification>(); // Returnerar en tom lista istället för null om svaret är tomt.
  }
  
  //GET - Hämtar bara olästa notiser 
  public async Task<List<Notification>> GetUserUnreadAsync(int userId)
  {
    var response = await
      _client.GetAsync($"api/notifications/user/{userId}/unread");
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<List<Notification>>() ??
           new List<Notification>();
  }
  
  // PUT - markera notiser som läst eller oläst (kräver API nyckel)
  public async Task UpdateReadStatusAsync(int notificationId, bool isRead)
  {
    var request = new HttpRequestMessage(HttpMethod.Put, $"api/notifications/{notificationId}"); // HTTP-anrop med metod och URL
    request.Headers.Add("X-API-Key", _apiKey); //Lägger till api-nyckeln som header
    request.Content = JsonContent.Create(new { isRead }); 
    var response = await _client.SendAsync(request); // Skickar det färdiga anropet
    response.EnsureSuccessStatusCode();
  }
  
  // DELETE - Ta bort en notis (kräver API nyckel)
  public async Task DeleteNotificationAsync(int notificationId)
  {
    var request = new HttpRequestMessage(HttpMethod.Delete, $"api/notifications/{notificationId}");
    request.Headers.Add("X-API-Key", _apiKey);
    var response = await _client.SendAsync(request);
    response.EnsureSuccessStatusCode();
  }
}