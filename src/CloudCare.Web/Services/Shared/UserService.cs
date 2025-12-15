using System.Net.Http.Json;
using CloudCare.Shared.DTOs.User;

namespace CloudCare.Web.Services.Shared;

public class UserService
{
    private readonly HttpClient _http;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> UserExistsAsync()
    {
        try
        {
            var response = await _http.GetAsync("api/users/exists");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            // You might want to handle other status codes (e.g., 404 Not Found) differently
            return false;
        }
        catch (HttpRequestException ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    public async Task<UserForReadDTO?> RegisterUserAsync(UserForCreationDto user)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/users/register", user);
            response.EnsureSuccessStatusCode(); // Throws if not a success code.
            return await response.Content.ReadFromJsonAsync<UserForReadDTO>();
        }
        catch (HttpRequestException ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}