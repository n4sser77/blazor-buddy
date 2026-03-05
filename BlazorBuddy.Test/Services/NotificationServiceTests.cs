using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Services;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.Test.Services;

public class NotificationServiceTests
{
    private readonly INotificationService _sut;

    public NotificationServiceTests()
    {
        _sut = new NotificationService();
    }

    [Theory]
    [InlineData("Test Notification", "Test Description", NotificationType.ChatMessage)]
    [InlineData("Group Invite", "You've been invited", NotificationType.ChatGroupInvite)]
    [InlineData("Friend Request", "New friend request", NotificationType.FriendRequest)]
    public void ShowNotification_RaisesOnNotificationReceivedEvent(
        string expectedTitle, 
        string expectedDescription, 
        NotificationType expectedType)
    {
        // Arrange
        Notification? actualNotification = null;

        _sut.OnNotificationReceived += (notification) =>
        {
            actualNotification = notification;
        };

        // Act
        _sut.ShowNotification(expectedTitle, expectedDescription, expectedType);

        // Assert
        Assert.NotNull(actualNotification);
        Assert.Equal(expectedTitle, actualNotification.Title);
        Assert.Equal(expectedDescription, actualNotification.Description);
        Assert.Equal(expectedType, actualNotification.Type);
        Assert.NotEqual(Guid.Empty, actualNotification.Id);
    }

    [Theory]
    [InlineData(NotificationType.ChatMessage, "New Message", "You have a new message")]
    [InlineData(NotificationType.ChatGroupInvite, "Group Invite", "You've been invited to join a group")]
    [InlineData(NotificationType.FriendRequest, "Friend Request", "Someone sent you a friend request")]
    public void ShowNotification_WithDifferentTypes_CreatesCorrectNotificationType(
        NotificationType expectedType, 
        string expectedTitle, 
        string expectedDescription)
    {
        // Arrange
        Notification? actualNotification = null;

        _sut.OnNotificationReceived += (notification) =>
        {
            actualNotification = notification;
        };

        // Act
        _sut.ShowNotification(expectedTitle, expectedDescription, expectedType);

        // Assert
        Assert.NotNull(actualNotification);
        Assert.Equal(expectedType, actualNotification.Type);
        Assert.Equal(expectedTitle, actualNotification.Title);
        Assert.Equal(expectedDescription, actualNotification.Description);
    }

    [Theory]
    [InlineData("Test", "Test Description", NotificationType.ChatMessage)]
    [InlineData("Another Test", "Another Description", NotificationType.ChatGroupInvite)]
    public void ShowNotification_WhenNoSubscribers_DoesNotThrowException(
        string testTitle, 
        string testDescription, 
        NotificationType testType)
    {
        // Arrange & Act
        var actualException = Record.Exception(() => _sut.ShowNotification(testTitle, testDescription, testType));

        // Assert
        Assert.Null(actualException);
    }

    [Theory]
    [InlineData("First Notification", "First Description", NotificationType.ChatMessage, 2)]
    [InlineData("Second Notification", "Second Description", NotificationType.FriendRequest, 3)]
    public void ShowNotification_GeneratesUniqueId_ForEachNotification(
        string testTitle, 
        string testDescription, 
        NotificationType testType, 
        int expectedCallCount)
    {
        // Arrange
        var actualReceivedIds = new List<Guid>();

        _sut.OnNotificationReceived += (notification) =>
        {
            actualReceivedIds.Add(notification.Id);
        };

        // Act
        for (int i = 0; i < expectedCallCount; i++)
        {
            _sut.ShowNotification(testTitle, testDescription, testType);
        }

        // Assert
        Assert.Equal(expectedCallCount, actualReceivedIds.Count);
        
        var actualDistinctCount = actualReceivedIds.Distinct().Count();
        Assert.Equal(expectedCallCount, actualDistinctCount); // All IDs are unique
        
        Assert.All(actualReceivedIds, actualId => Assert.NotEqual(Guid.Empty, actualId));
    }
}
