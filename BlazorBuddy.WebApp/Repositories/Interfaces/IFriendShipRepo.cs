namespace BlazorBuddy.WebApp.Repositories.Interfaces
{
    public interface IFriendShipRepo
    {
        Task<bool> RemoveFriendAsync(string userId, string friendId);
        Task<List<string>> GetFriendsAsync(string userId);
        Task<List<string>>  GetFriendRequestsAsync(string userId);
        Task<bool> SendFriendRequestAsync(string userId, string friendId);
        Task<bool> AcceptFriendRequestAsync(string userId, string friendId);
        Task<bool> DeclineFriendRequestAsync(string userId, string friendId);
        Task<bool> CheckIfFriendAsync(string userId, string friendId);
        Task<List<string>> GetSentRequestsAsync(string userId);
    }
}
