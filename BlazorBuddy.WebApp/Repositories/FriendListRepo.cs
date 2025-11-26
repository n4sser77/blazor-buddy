using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;

namespace BlazorBuddy.WebApp.Repositories
{
    public class FriendListRepo : IFriendlist
    {
        private readonly ApplicationDbContext _context;

        public FriendListRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> AddFriendAsync(string userId, string friendId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemoveFriendAsync(string userId, string friendId)
        {
            throw new NotImplementedException();
        }
        public Task<List<string>> GetFriendsAsync(string userId)
        {
            throw new NotImplementedException();
        }
        public Task<List<string>> GetFriendRequestsAsync(string userId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> SendFriendRequestAsync(string userId, string friendId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcceptFriendRequestAsync(string userId, string friendId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeclineFriendRequestAsync(string userId, string friendId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> CheckIfFriendAsync(string userId, string friendId)
        {
            throw new NotImplementedException();
        }
    }
}
