using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Services
{
    public class UserProfileRepo : IUserRepo
    {
        private readonly ApplicationDbContext _ctx;
        public UserProfileRepo(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<UserProfile>> GetAllUsers()
        {
            var users = await _ctx.UserProfiles.ToListAsync();
            return users ?? [];
        }

        public async Task<UserProfile?> GetUserById(string id)
        {
            var userProfile = await _ctx.UserProfiles.FirstOrDefaultAsync(p => p.Id == id);
            return userProfile;
        }

        public async Task<UserProfile?> UpdateUserProfile(UserProfile user)
        {
            var currentUser = await _ctx.UserProfiles.FirstOrDefaultAsync(u => user.Id == u.Id);

            return currentUser;
        }
    }
}
