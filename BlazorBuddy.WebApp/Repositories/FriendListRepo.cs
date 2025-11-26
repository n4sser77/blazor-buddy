using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlazorBuddy.Core.Models;

namespace BlazorBuddy.WebApp.Repositories
{
    public class FriendListRepo : IFriendlist
    {
        private readonly ApplicationDbContext _context;

        public FriendListRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFriendAsync(string userId, string friendId)
        {
            var addFriend = await _context.FriendLists
             .FirstOrDefaultAsync(fl =>
                (fl.UserId == userId && fl.FriendId == friendId) ||
                (fl.UserId == friendId && fl.FriendId == userId));

            if (addFriend != null)
                return false;

            var newFriend = new FriendList
            {
                UserId = userId,
                FriendId = friendId,
                Status = FriendStatus.Accepted,
                SentAt = DateTime.UtcNow
            };

            _context.FriendLists.Add(newFriend);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveFriendAsync(string userId, string friendId)
        {
            var friendship = await _context.FriendLists
             .FirstOrDefaultAsync(fl =>
                (fl.UserId == userId && fl.FriendId == friendId) ||
                (fl.UserId == friendId && fl.FriendId == userId));

            if (friendship == null)
                return false;

            _context.FriendLists.Remove(friendship);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<string>> GetFriendsAsync(string userId)
        {
            var friendIds = await _context.FriendLists
                .AsNoTracking()
                .Where(fl => (fl.UserId == userId || fl.FriendId == userId) && fl.Status == FriendStatus.Accepted)
                .Select(fl => fl.UserId == userId ? fl.FriendId : fl.UserId)
                .Distinct()
                .ToListAsync();
            return friendIds;
        }
        public async Task<List<string>> GetFriendRequestsAsync(string userId)
        {
            var requesterIds = await _context.FriendLists
                .AsNoTracking()
                .Where(fl => fl.FriendId == userId && fl.Status == FriendStatus.Pending)
                .Select(fl => fl.UserId)
                .ToListAsync();

            return requesterIds;
        }
        public async Task<bool> SendFriendRequestAsync(string userId, string friendId)
        {

            if (userId == friendId)
                return false;

            var isBlocked = await _context.FriendLists
                .AsNoTracking()
                .AnyAsync(fl =>
                    ((fl.UserId == userId && fl.FriendId == friendId) ||
                     (fl.UserId == friendId && fl.FriendId == userId)) &&
                    fl.Status == FriendStatus.Blocked);
            if (isBlocked)
                return false;

            var alreadyFriends = await _context.FriendLists
                .AsNoTracking()
                .AnyAsync(fl =>
                    ((fl.UserId == userId && fl.FriendId == friendId) ||
                     (fl.UserId == friendId && fl.FriendId == userId)) &&
                    fl.Status == FriendStatus.Accepted);
            if (alreadyFriends)
                return false;

            var pendingSameDirection = await _context.FriendLists
                .AsNoTracking()
                .AnyAsync(fl =>
                    fl.UserId == userId &&
                    fl.FriendId == friendId &&
                    fl.Status == FriendStatus.Pending);
            if (pendingSameDirection)
                return false;

            var pendingReverse = await _context.FriendLists
                .AsNoTracking()
                .AnyAsync(fl =>
                    fl.UserId == friendId &&
                    fl.FriendId == userId &&
                    fl.Status == FriendStatus.Pending);
            if (pendingReverse)
                return false;

            var request = new FriendList
            {
                UserId = userId,
                FriendId = friendId,
                Status = FriendStatus.Pending,
                SentAt = DateTime.UtcNow
            };

            _context.FriendLists.Add(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AcceptFriendRequestAsync(string userId, string friendId)
        {
            var acceptFriendRequest = await _context.FriendLists
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
            var declineFriendRequest = await _context.FriendLists
                .FirstOrDefaultAsync(fr =>
                    fr.UserId == friendId &&
                    fr.FriendId == userId &&
                    fr.Status == FriendStatus.Pending
                    );

            if (declineFriendRequest == null)
                return false;

            _context.FriendLists.Remove(declineFriendRequest);
            await _context.SaveChangesAsync();
            return true;
        }
        public Task<bool> CheckIfFriendAsync(string userId, string friendId)
        {
            return _context.FriendLists
                .AsNoTracking()
                .AnyAsync(fl =>
                    ((fl.UserId == userId && fl.FriendId == friendId) ||
                     (fl.UserId == friendId && fl.FriendId == userId)) &&
                    fl.Status == FriendStatus.Accepted);
        }
    }
}
