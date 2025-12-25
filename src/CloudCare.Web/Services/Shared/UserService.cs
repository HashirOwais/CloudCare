using System.Net.Http.Json;
using CloudCare.Shared.DTOs.User;
using Microsoft.Extensions.Logging;

namespace CloudCare.Web.Services.Shared;

public class UserService
{
    private readonly HttpClient _http;
    private readonly ILogger<UserService> _logger;

    public UserService(HttpClient http, ILogger<UserService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<bool> UserExistsAsync()
    {
        _logger.LogInformation("Checking if user exists via API.");
        try
        {
            var response = await _http.GetAsync("api/users/exists");
            if (response.IsSuccessStatusCode)
            {
                var userExists = await response.Content.ReadFromJsonAsync<bool>();
                _logger.LogInformation("User exists check completed. User exists: {UserExists}", userExists);
                return userExists;
            }
            
            _logger.LogWarning("User exists check failed with status code: {StatusCode}", response.StatusCode);
            return false;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "An error occurred while checking if user exists.");
            return false;
        }
    }

    public async Task<UserForReadDTO?> RegisterUserAsync(UserForCreationDto user)
    {
        _logger.LogInformation("Registering a new user via API.");
        try
        {
            var response = await _http.PostAsJsonAsync("api/users/register", user);
            response.EnsureSuccessStatusCode(); // Throws if not a success code.
            var registeredUser = await response.Content.ReadFromJsonAsync<UserForReadDTO>();
            _logger.LogInformation("Successfully registered a new user with Auth0 ID: {Auth0Id}", registeredUser?.Auth0Id);
            return registeredUser;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "An error occurred while registering a new user.");
            return null;
        }
    }
}