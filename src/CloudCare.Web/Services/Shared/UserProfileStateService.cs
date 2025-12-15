using CloudCare.Web.Services.Shared;

namespace CloudCare.Web.Services;

// Services/UserProfileStateService.cs
public class UserProfileStateService
{
    private readonly UserService _userService;
    private bool? _hasProfile;
    private Task<bool>? _checkTask;

    public UserProfileStateService(UserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> HasProfileAsync()
    {
        // Return cached value if available
        if (_hasProfile.HasValue)
            return _hasProfile.Value;

        // Prevent duplicate calls if already checking
        if (_checkTask != null)
            return await _checkTask;

        // Make the API call once
        _checkTask = _userService.UserExistsAsync();
        _hasProfile = await _checkTask;
        _checkTask = null;

        return _hasProfile.Value;
    }

    public void ClearCache()
    {
        _hasProfile = null;
    }
}
