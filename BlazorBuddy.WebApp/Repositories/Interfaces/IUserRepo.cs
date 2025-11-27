using BlazorBuddy.Models;

namespace BlazorBuddy.Core.Interfaces
{
    public interface IUserRepo
    {
        Task<UserProfile?> GetUserById(string id);
        Task<List<UserProfile>> GetAllUsers();
        Task<UserProfile?> UpdateUserProfile(UserProfile user);

    }
}