using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserProfile?> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return await _userRepo.GetUserById(id);
        }

        public async Task<List<UserProfile>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsers();
        }

        public async Task<UserProfile?> UpdateUserProfileAsync(UserProfile user)
        {
            if (user == null)
                return null;

            return await _userRepo.UpdateUserProfile(user);
        }
    }
}
