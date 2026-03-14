using Bibliotekssystem_T9_App.Dtos;

namespace Bibliotekssystem_T9_App.Services;

// Service som anropar UserService API
public class UserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Users/login", dto);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<UserDto>>("/api/Users");

        return response ?? new List<UserDto>();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/Users/{id}");
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> CreateUserAsync(CreateUserDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Users", dto);
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateUserAsync(EditUserDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/{dto.Id}", dto);
        return response.IsSuccessStatusCode;
    }
}