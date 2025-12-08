using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBuddy.Test.Integration
{
    public class FriendShipIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _context;
        private readonly IFriendShipService _friendService;

        public FriendShipIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _friendService = _scope.ServiceProvider.GetRequiredService<IFriendShipService>();
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }

        [Fact]
        public async Task SendFriendRequest_ShouldCreatePendingRequest()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-1", DisplayName = "Anna" };
            var user2 = new UserProfile { Id = "user-2", DisplayName = "Björn" };
            _context.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);

            // Assert
            Assert.True(result);
            var receivedRequests = await _friendService.GetReceivedRequestsAsync(user2.Id);
            Assert.Single(receivedRequests);
            Assert.Equal(user1.Id, receivedRequests[0].Id);
        }

        [Fact]
        public async Task AcceptFriendRequest_ShouldAddToFriendsList()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-3", DisplayName = "Carl" };
            var user2 = new UserProfile { Id = "user-4", DisplayName = "Diana" };
            _context.AddRange(user1, user2);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            // Act
            var acceptResult = await _friendService.AcceptRequestAsync(user2.Id, user1.Id);
            // Assert
            Assert.True(acceptResult);
            var friendsOfUser1 = await _friendService.GetFriendsAsync(user1.Id);
            var friendsOfUser2 = await _friendService.GetFriendsAsync(user2.Id);
            Assert.Contains(friendsOfUser1, f => f.Id == user2.Id);
            Assert.Contains(friendsOfUser2, f => f.Id == user1.Id);
        }
        [Fact]
        public async Task DeclineFriendRequest_ShouldNotAddToFriendsList()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-5", DisplayName = "Eva" };
            var user2 = new UserProfile { Id = "user-6", DisplayName = "Fredrik" };
            _context.AddRange(user1, user2);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            // Act
            var declineResult = await _friendService.DeclineRequestAsync(user2.Id, user1.Id);
            // Assert
            Assert.True(declineResult);
            var friendsOfUser1 = await _friendService.GetFriendsAsync(user1.Id);
            var friendsOfUser2 = await _friendService.GetFriendsAsync(user2.Id);
            Assert.DoesNotContain(friendsOfUser1, f => f.Id == user2.Id);
            Assert.DoesNotContain(friendsOfUser2, f => f.Id == user1.Id);
        }
        [Fact]
        public async Task RemoveFriend_ShouldNoLongerBeFriends()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-7", DisplayName = "Gustav" };
            var user2 = new UserProfile { Id = "user-8", DisplayName = "Helena" };
            _context.AddRange(user1, user2);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            await _friendService.AcceptRequestAsync(user2.Id, user1.Id);
            // Act
            var removeResult = await _friendService.RemoveFriendAsync(user1.Id, user2.Id);
            // Assert
            Assert.True(removeResult);
            var friendsOfUser1 = await _friendService.GetFriendsAsync(user1.Id);
            var friendsOfUser2 = await _friendService.GetFriendsAsync(user2.Id);
            Assert.DoesNotContain(friendsOfUser1, f => f.Id == user2.Id);
            Assert.DoesNotContain(friendsOfUser2, f => f.Id == user1.Id);

        }
        [Fact]
        public async Task CheckIfFriendAsync_ShouldReturnCorrectStatus()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-9", DisplayName = "Ingrid" };
            var user2 = new UserProfile { Id = "user-10", DisplayName = "Johan" };
            _context.AddRange(user1, user2);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            await _friendService.AcceptRequestAsync(user2.Id, user1.Id);
            // Act
            var areFriends = await _friendService.CheckIfFriendAsync(user1.Id, user2.Id);
            // Assert
            Assert.True(areFriends);

        }
        [Fact]
        public async Task GetAvailableUsersForFriendRequest_ShouldReturnCorrectUsers()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-11", DisplayName = "Karin" };
            var user2 = new UserProfile { Id = "user-12", DisplayName = "Lars" };
            var user3 = new UserProfile { Id = "user-13", DisplayName = "Maria" };
            _context.AddRange(user1, user2, user3);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            // Act
            var availableUsers = await _friendService.GetAvailableUsersForFriendRequestAsync(user1.Id);
            // Assert
            Assert.Single(availableUsers);
            Assert.Equal(user3.Id, availableUsers[0].Id);
        }
        [Fact]
        public async Task GetFriendsAsync_ShouldReturnAllFriends()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-14", DisplayName = "Nils" };
            var user2 = new UserProfile { Id = "user-15", DisplayName = "Olivia" };
            var user3 = new UserProfile { Id = "user-16", DisplayName = "Per" };
            _context.AddRange(user1, user2, user3);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            await _friendService.AcceptRequestAsync(user2.Id, user1.Id);
            await _friendService.SendFriendRequestAsync(user1.Id, user3.Id);
            await _friendService.AcceptRequestAsync(user3.Id, user1.Id);
            // Act
            var friendsOfUser1 = await _friendService.GetFriendsAsync(user1.Id);
            // Assert
            Assert.Equal(2, friendsOfUser1.Count);
            Assert.Contains(friendsOfUser1, f => f.Id == user2.Id);
            Assert.Contains(friendsOfUser1, f => f.Id == user3.Id);
        }
        [Fact]
        public async Task GetReceivedRequestsAsync_ShouldReturnAllReceivedRequests()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-17", DisplayName = "Rasmus" };
            var user2 = new UserProfile { Id = "user-18", DisplayName = "Sofia" };
            _context.AddRange(user1, user2);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            // Act
            var receivedRequests = await _friendService.GetReceivedRequestsAsync(user2.Id);
            // Assert
            Assert.Single(receivedRequests);
            Assert.Equal(user1.Id, receivedRequests[0].Id);
        }
        [Fact]
        public async Task GetSentRequestsAsync_ShouldReturnAllSentRequests()
        {
            // Arrange
            var user1 = new UserProfile { Id = "user-19", DisplayName = "Sara" };
            var user2 = new UserProfile { Id = "user-20", DisplayName = "Tobias" };
            _context.AddRange(user1, user2);
            await _context.SaveChangesAsync();
            await _friendService.SendFriendRequestAsync(user1.Id, user2.Id);
            // Act
            var sentRequests = await _friendService.GetSentRequestsAsync(user1.Id);
            // Assert
            Assert.Single(sentRequests);
            Assert.Equal(user2.Id, sentRequests[0].Id);
        }

    }
}
