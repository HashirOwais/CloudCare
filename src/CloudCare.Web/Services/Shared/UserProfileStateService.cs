using CloudCare.Web.Services.Shared;
using Microsoft.Extensions.Logging;

namespace CloudCare.Web.Services;

// Services/UserProfileStateService.cs
public class UserProfileStateService
{
    private readonly UserService _userService;
    private bool? _hasProfile;
    private Task<bool>? _checkTask;
    private readonly ILogger<UserProfileStateService> _logger;

    public UserProfileStateService(UserService userService, ILogger<UserProfileStateService> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<bool> HasProfileAsync()
    {
        _logger.LogInformation("Checking if user has a profile.");
        // Return cached value if available
        if (_hasProfile.HasValue)
        {
            _logger.LogInformation("Returning cached profile status: {HasProfile}", _hasProfile.Value);
            return _hasProfile.Value;
        }

        // Prevent duplicate calls if already checking
        if (_checkTask != null)
        {
            _logger.LogInformation("Profile check already in progress, awaiting result.");
            return await _checkTask;
        }

        // Make the API call once
        _logger.LogInformation("No cached profile status, making API call to check if user exists.");
        _checkTask = _userService.UserExistsAsync();
        _hasProfile = await _checkTask;
        _checkTask = null;

        _logger.LogInformation("API call completed, user profile status: {HasProfile}", _hasProfile.Value);
        return _hasProfile.Value;
    }

    public void ClearCache()
    {
        _logger.LogInformation("Clearing user profile cache.");
        _hasProfile = null;
    }
}
