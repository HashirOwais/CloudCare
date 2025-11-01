using CloudCare.API.DTOs;

namespace CloudCare.API.Services;

public interface IUserService
{
    Task<bool> CheckLocalUserExistsAsync(string auth0UserId);
    Task<UserForReadDTO>  RegisterUserAsync(UserForCreationDto userRegistrationDto);
    
}