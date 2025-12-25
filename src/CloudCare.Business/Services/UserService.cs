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
        _logger.LogInformation("Checking if local user exists with Auth0 ID: {Auth0UserId}", auth0UserId);
        var userExists = await _userRepository.IsUserExistsAsync(auth0UserId);
        _logger.LogInformation("User with Auth0 ID: {Auth0UserId} exists: {UserExists}", auth0UserId, userExists);
        return userExists;
    }

    public async Task<UserForReadDTO> RegisterUserAsync(UserForCreationDto userRegistrationDto)
    {
        _logger.LogInformation("Attempting to register a new user");
        // Map DTO to User model
        var userModel = _mapper.Map<User>(userRegistrationDto);

        // Add the new user model to the database.
        await _userRepository.AddUserAsync(userModel);

        // Map back to read DTO after DB save
        var userReadDto = _mapper.Map<UserForReadDTO>(userModel);
        _logger.LogInformation("Successfully registered new user with ID: {UserId}", userModel.Id);
        return userReadDto;
    }

    public async Task<UserForReadDTO> GetUserByAuth0IdAsync(string auth0UserId)
    {
        _logger.LogWarning("GetUserByAuth0IdAsync is not implemented.");
        throw new NotImplementedException();
    }

    public async Task<int> GetLocalUserIdAsync(string auth0UserId)
    {
        _logger.LogWarning("GetLocalUserIdAsync is not implemented.");
        throw new NotImplementedException();
    }
}