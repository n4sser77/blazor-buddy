using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services
{
    public class FriendListService : IFriendListService
    {
        private readonly IFriendlistRepo _friendlistRepo;
        private readonly IUserRepo _userRepo;

        public FriendListService(IFriendlistRepo friendlistRepo, IUserRepo userRepo)
        {
            _friendlistRepo = friendlistRepo;
            _userRepo = userRepo;
        }

        public async Task<bool> SendFriendRequestAsync(string userId, string friendId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(friendId))
                return false;

            if (userId == friendId)
                return false;

            if (await _friendlistRepo.CheckIfFriendAsync(userId, friendId))
                return false;

            return await _friendlistRepo.SendFriendRequestAsync(userId, friendId);
        }
        public async Task<bool> RemoveFriendAsync(string userId, string friendId)
        {
            return await _friendlistRepo.RemoveFriendAsync(userId, friendId);
        }
        public async Task<List<UserProfile>> GetFriendsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new List<UserProfile>();

            var friendIds = await _friendlistRepo.GetFriendsAsync(userId);
            if (!friendIds.Any())
                return new List<UserProfile>();

            var allUsers = await _userRepo.GetAllUsers();
            return allUsers.Where(u => friendIds.Contains(u.Id)).ToList();
        }
        public async Task<List<UserProfile>> GetReceivedRequestsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new List<UserProfile>();

            var requestIds = await _friendlistRepo.GetFriendRequestsAsync(userId);
            if (!requestIds.Any())
                return new List<UserProfile>();

            var allUsers = await _userRepo.GetAllUsers();
            return allUsers.Where(u => requestIds.Contains(u.Id)).ToList();
        }

        public async Task<List<UserProfile>> GetSentRequestsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new List<UserProfile>();

            var sentRequestIds = await _friendlistRepo.GetSentRequestsAsync(userId);
            if (!sentRequestIds.Any())
                return new List<UserProfile>();

            var allUsers = await _userRepo.GetAllUsers();
            return allUsers.Where(u => sentRequestIds.Contains(u.Id)).ToList();
        }
        public async Task<bool> AcceptRequestAsync(string userId, string friendId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(friendId))
                return false;

            return await _friendlistRepo.AcceptFriendRequestAsync(userId, friendId);
        }

        public async Task<bool> DeclineRequestAsync(string userId, string friendId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(friendId))
                return false;

            return await _friendlistRepo.DeclineFriendRequestAsync(userId, friendId);
        }

        public async Task<bool> CheckIfFriendAsync(string userId, string friendId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(friendId))
                return false;

            return await _friendlistRepo.CheckIfFriendAsync(userId, friendId);
        }
    }
}
