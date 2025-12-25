using CloudCare.Business.Repositories.Interfaces;
using AutoMapper;
using CloudCare.Shared.DTOs.User;
using CloudCare.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CloudCare.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> CheckLocalUserExistsAsync(string auth0UserId)
    {
        _logger.LogInformation("CheckLocalUserExistsAsync called for auth0UserId: {auth0UserId}", auth0UserId);
        var userExists = await _userRepository.IsUserExistsAsync(auth0UserId);
        _logger.LogInformation("CheckLocalUserExistsAsync finished for auth0UserId: {auth0UserId}, user exists: {userExists}", auth0UserId, userExists);
        return userExists;
    }

    public async Task<UserForReadDTO> RegisterUserAsync(UserForCreationDto userRegistrationDto)
    {
        _logger.LogInformation("RegisterUserAsync called for user with email: {email}", userRegistrationDto.Email);
        // Map DTO to User model
        var userModel = _mapper.Map<User>(userRegistrationDto);

        // Add the new user model to the database.
        await _userRepository.AddUserAsync(userModel);

        // Map back to read DTO after DB save
        var userReadDto = _mapper.Map<UserForReadDTO>(userModel);
        _logger.LogInformation("RegisterUserAsync finished for user with email: {email}, created user with id: {id}", userRegistrationDto.Email, userReadDto.Auth0Id);
        return userReadDto;
    }

    public async Task<UserForReadDTO> GetUserByAuth0IdAsync(string auth0UserId)
    {
        _logger.LogInformation("GetUserByAuth0IdAsync called for auth0UserId: {auth0UserId}", auth0UserId);
        throw new NotImplementedException();
    }

    public async Task<int> GetLocalUserIdAsync(string auth0UserId)
    {
        _logger.LogInformation("GetLocalUserIdAsync called for auth0UserId: {auth0UserId}", auth0UserId);
        throw new NotImplementedException();
    }
}