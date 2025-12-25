using System.Security.Claims;
using System.Threading.Tasks;
using CloudCare.Business.Repositories.Interfaces;
using CloudCare.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CloudCare.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string? GetAuth0UserId()
        {
            var auth0Id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(auth0Id))
            {
                _logger.LogWarning("Auth0 User ID not found in HttpContext.");
            }
            else
            {
                _logger.LogInformation("Retrieved Auth0 User ID: {Auth0Id}", auth0Id);
            }
            return auth0Id;
        }

        public async Task<int?> GetCurrentUserId()
        {
            var auth0Id = GetAuth0UserId();
            if (string.IsNullOrEmpty(auth0Id))
            {
                _logger.LogWarning("Cannot get current user ID because Auth0 ID is null or empty.");
                return null;
            }
            
            _logger.LogInformation("Getting current user ID for Auth0 ID: {Auth0Id}", auth0Id);
            var user = await _userRepository.GetUserByAuth0IdAsync(auth0Id);

            if (user == null)
            {
                _logger.LogWarning("User not found for Auth0 ID: {Auth0Id}", auth0Id);
                return null;
            }
            
            _logger.LogInformation("Found user with ID: {UserId} for Auth0 ID: {Auth0Id}", user.Id, auth0Id);
            return user.Id;
        }

        public async Task<User?> GetUserByAuth0Id(string auth0Id)
        {
            _logger.LogInformation("Getting user by Auth0 ID: {Auth0Id}", auth0Id);
            var user = await _userRepository.GetUserByAuth0IdAsync(auth0Id);
            if (user == null)
            {
                _logger.LogWarning("User not found for Auth0 ID: {Auth0Id}", auth0Id);
            }
            else
            {
                _logger.LogInformation("Found user with ID: {UserId} for Auth0 ID: {Auth0Id}", user.Id, auth0Id);
            }
            return user;
        }
    }
}
