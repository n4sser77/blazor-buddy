using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfile?> GetUserByIdAsync(string id);
        Task<List<UserProfile>> GetAllUsersAsync();
        Task<UserProfile?> UpdateUserProfileAsync(UserProfile user);
    }
}
