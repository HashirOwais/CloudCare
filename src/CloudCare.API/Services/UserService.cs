using System.Security.Claims;
using System.Threading.Tasks;
using CloudCare.Business.Repositories.Interfaces;
using CloudCare.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace CloudCare.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetAuth0UserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<int?> GetCurrentUserId()
        {
            var auth0Id = GetAuth0UserId();
            if (string.IsNullOrEmpty(auth0Id))
            {
                return null;
            }
            var user = await _userRepository.GetUserByAuth0IdAsync(auth0Id);
            return user?.Id;
        }

        public async Task<User?> GetUserByAuth0Id(string auth0Id)
        {
            return await _userRepository.GetUserByAuth0IdAsync(auth0Id);
        }
    }
}
