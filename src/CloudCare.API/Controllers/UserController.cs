using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CloudCare.Shared.DTOs.User;
using CloudCare.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

// a claim is a statement about a user, it can be a name, role, age, etc. We want the sub

namespace CloudCare.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // 2. Extract the unique Auth0 ID (the 'sub' claim)
        // Auth0 maps the 'sub' claim to the ClaimTypes.NameIdentifier constant
        protected string? GetAuth0UserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserForReadDTO>> RegisterUser([FromBody] UserForCreationDto dto)
        {
            var auth0UserId = GetAuth0UserId();
            if (string.IsNullOrEmpty(auth0UserId))
            {
                _logger.LogWarning("User ID claim not found in token during registration.");
                return BadRequest("User ID claim not found in token.");
            }

            _logger.LogInformation("Attempting to register user with Auth0 ID: {Auth0UserId}", auth0UserId);
            // 1. Await the service call to get the UserForReadDTO
            var readDto = await _userService.RegisterUserAsync(dto);

            // 2. Return the DTO wrapped in an Ok result.
            // The framework handles serializing the DTO.
            _logger.LogInformation("Successfully registered user with Auth0 ID: {Auth0UserId}", auth0UserId);
            return Ok(readDto);
        }

        [HttpGet("exists")]
        public async Task<ActionResult<bool>> UserExists()
        {
            // 1. Check if the user is authenticated (i.e., if the token was valid)

            var auth0UserId = GetAuth0UserId();

            if (string.IsNullOrEmpty(auth0UserId))
            {
                _logger.LogWarning("User ID claim not found in token when checking for existence.");
                return BadRequest("User ID claim not found in token.");
            }
            
            _logger.LogInformation("Checking if user exists with Auth0 ID: {Auth0UserId}", auth0UserId);
            var userExistsInLocalDB = await _userService.CheckLocalUserExistsAsync(auth0UserId);

            // The Ok() method now serializes the simple 'bool' value.
            _logger.LogInformation("User with Auth0 ID: {Auth0UserId} exists: {UserExists}", auth0UserId, userExistsInLocalDB);
            return Ok(userExistsInLocalDB);
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<bool> UserExistsNoAuth([FromQuery] string auth0UserId)
        {
            _logger.LogInformation("Checking if user exists with Auth0 ID: {Auth0UserId} (No Auth)", auth0UserId);
            // 1. Now, call your User Microservice logic (or Repository) here
            // var userExistsInLocalDB = _userService.CheckLocalUserExists(auth0UserId);

            // return Ok(userExistsInLocalDB);
            return Ok(true); // Placeholder for testing
        }
    }
}