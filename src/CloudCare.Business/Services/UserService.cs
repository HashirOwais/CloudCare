using CloudCare.Business.Repositories.Interfaces;
using AutoMapper;
using CloudCare.Shared.DTOs.User;
using CloudCare.Shared.Models;

namespace CloudCare.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool> CheckLocalUserExistsAsync(string auth0UserId)
    {
        var userExists = await _userRepository.IsUserExistsAsync(auth0UserId);
        return userExists;
    }

    public async Task<UserForReadDTO> RegisterUserAsync(UserForCreationDto userRegistrationDto)
    {
        // Map DTO to User model
        var userModel = _mapper.Map<User>(userRegistrationDto);

        // Add the new user model to the database.
        await _userRepository.AddUserAsync(userModel);

        // Map back to read DTO after DB save
        var userReadDto = _mapper.Map<UserForReadDTO>(userModel);
        return userReadDto;
    }

    public async Task<UserForReadDTO> GetUserByAuth0IdAsync(string auth0UserId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetLocalUserIdAsync(string auth0UserId)
    {
        throw new NotImplementedException();
    }
}