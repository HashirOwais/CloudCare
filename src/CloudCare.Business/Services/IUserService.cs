using CloudCare.Business.DTOs;

namespace CloudCare.Business.Services;

public interface IUserService
{
    Task<bool> CheckLocalUserExistsAsync(string auth0UserId);
    Task<UserForReadDTO> RegisterUserAsync(UserForCreationDto userRegistrationDto);

    Task<UserForReadDTO> GetUserByAuth0IdAsync(string auth0UserId);

    Task<int> GetLocalUserIdAsync(string auth0UserId);
}
