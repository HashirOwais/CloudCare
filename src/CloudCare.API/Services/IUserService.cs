using System.Security.Claims;
using System.Threading.Tasks;
using CloudCare.Shared.Models;

namespace CloudCare.API.Services
{
    public interface IUserService
    {
        Task<int?> GetCurrentUserId();
        Task<User?> GetUserByAuth0Id(string auth0Id);
    }
}
