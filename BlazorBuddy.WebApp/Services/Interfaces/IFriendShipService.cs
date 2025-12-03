using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services.Interfaces
{
    public interface IFriendShipService
    {
        Task<List<UserProfile>> GetFriendsAsync(string userId);
        Task<List<UserProfile>> GetReceivedRequestsAsync(string userId);
        Task<List<UserProfile>> GetSentRequestsAsync(string userId);
        Task<bool> SendFriendRequestAsync(string userId, string friendId);
        Task<bool> AcceptRequestAsync(string userId, string friendId);
        Task<bool> DeclineRequestAsync(string userId, string friendId);
        Task<bool> RemoveFriendAsync(string userId, string friendId);
        Task<bool> CheckIfFriendAsync(string userId, string friendId);
    }
}
