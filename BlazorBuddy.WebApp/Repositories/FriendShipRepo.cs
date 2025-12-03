using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlazorBuddy.Core.Models;

namespace BlazorBuddy.WebApp.Repositories
{
    public class FriendShipRepo : IFriendShipRepo
    {
        private readonly ApplicationDbContext _context;

        public FriendShipRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RemoveFriendAsync(string userId, string friendId)
        {
            var friendship = await _context.FriendShips
             .FirstOrDefaultAsync(fl =>
                (fl.UserId == userId && fl.FriendId == friendId) ||
                (fl.UserId == friendId && fl.FriendId == userId));

            if (friendship == null)
                return false;

            _context.FriendShips.Remove(friendship);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<string>> GetFriendsAsync(string userId)
        {
            var friendIds = await _context.FriendShips
                .AsNoTracking()
                .Where(fl => (fl.UserId == userId || fl.FriendId == userId) && fl.Status == FriendStatus.Accepted)
                .Select(fl => fl.UserId == userId ? fl.FriendId : fl.UserId)
                .Distinct()
                .ToListAsync();
            return friendIds;
        }
        public async Task<List<string>> GetFriendRequestsAsync(string userId)
        {
            var requesterIds = await _context.FriendShips
                .AsNoTracking()
                .Where(fl => fl.FriendId == userId && fl.Status == FriendStatus.Pending)
                .Select(fl => fl.UserId)
                .ToListAsync();

            return requesterIds;
        }
        public async Task<bool> SendFriendRequestAsync(string userId, string friendId)
        {
            var request = new FriendShip
            {
                UserId = userId,
                FriendId = friendId,
                Status = FriendStatus.Pending,
                SentAt = DateTime.UtcNow
            };

            _context.FriendShips.Add(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AcceptFriendRequestAsync(string userId, string friendId)
        {
            var acceptFriendRequest = await _context.FriendShips
                .FirstOrDefaultAsync(fr =>
                fr.UserId == friendId &&
                fr.FriendId == userId &&
                fr.Status == FriendStatus.Pending
                );
            if (acceptFriendRequest == null)
                return false;
            acceptFriendRequest.Status = FriendStatus.Accepted;
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> DeclineFriendRequestAsync(string userId, string friendId)
        {
            var declineFriendRequest = await _context.FriendShips
                .FirstOrDefaultAsync(fr =>
                    fr.UserId == friendId &&
                    fr.FriendId == userId &&
                    fr.Status == FriendStatus.Pending
                    );

            if (declineFriendRequest == null)
                return false;

            _context.FriendShips.Remove(declineFriendRequest);
            await _context.SaveChangesAsync();
            return true;
        }
        public Task<bool> CheckIfFriendAsync(string userId, string friendId)
        {
            return _context.FriendShips
                .AsNoTracking()
                .AnyAsync(fl =>
                    ((fl.UserId == userId && fl.FriendId == friendId) ||
                     (fl.UserId == friendId && fl.FriendId == userId)) &&
                    fl.Status == FriendStatus.Accepted);
        }

        public async Task<List<string>> GetSentRequestsAsync(string userId)
        {
            var sentRequestIds = await _context.FriendShips
                .AsNoTracking()
                .Where(fl => fl.UserId == userId && fl.Status == FriendStatus.Pending)
                .Select(fl => fl.FriendId)
                .ToListAsync();

            return sentRequestIds;
        }
    }
}
