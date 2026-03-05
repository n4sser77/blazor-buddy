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

    [Fact]
    public void ShowNotification_RaisesOnNotificationReceivedEvent()
    {
        // Arrange
        var expectedTitle = "Test Notification";
        var expectedDescription = "Test Description";
        var expectedType = NotificationType.ChatMessage;
        Notification? receivedNotification = null;

        _sut.OnNotificationReceived += (notification) =>
        {
            receivedNotification = notification;
        };

        // Act
        _sut.ShowNotification(expectedTitle, expectedDescription, expectedType);

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(expectedTitle, receivedNotification.Title);
        Assert.Equal(expectedDescription, receivedNotification.Description);
        Assert.Equal(expectedType, receivedNotification.Type);
        Assert.NotEqual(Guid.Empty, receivedNotification.Id);
    }

    [Theory]
    [InlineData(NotificationType.ChatMessage, "New Message", "You have a new message")]
    [InlineData(NotificationType.ChatGroupInvite, "Group Invite", "You've been invited to join a group")]
    [InlineData(NotificationType.FriendRequest, "Friend Request", "Someone sent you a friend request")]
    public void ShowNotification_WithDifferentTypes_CreatesCorrectNotificationType(
        NotificationType expectedType, 
        string title, 
        string description)
    {
        // Arrange
        Notification? actualNotification = null;

        _sut.OnNotificationReceived += (notification) =>
        {
            actualNotification = notification;
        };

        // Act
        _sut.ShowNotification(title, description, expectedType);

        // Assert
        Assert.NotNull(actualNotification);
        Assert.Equal(expectedType, actualNotification.Type);
        Assert.Equal(title, actualNotification.Title);
        Assert.Equal(description, actualNotification.Description);
    }

    [Fact]
    public void ShowNotification_WhenNoSubscribers_DoesNotThrowException()
    {
        // Arrange
        var title = "Test";
        var description = "Test";
        var type = NotificationType.ChatMessage;

        // Act & Assert
        var exception = Record.Exception(() => _sut.ShowNotification(title, description, type));
        Assert.Null(exception);
    }

    [Fact]
    public void ShowNotification_GeneratesUniqueId_ForEachNotification()
    {
        // Arrange
        var title = "Test Notification";
        var description = "Test Description";
        var type = NotificationType.ChatMessage;
        var receivedIds = new List<Guid>();

        _sut.OnNotificationReceived += (notification) =>
        {
            receivedIds.Add(notification.Id);
        };

        // Act
        _sut.ShowNotification(title, description, type);
        _sut.ShowNotification(title, description, type);

        // Assert
        Assert.Equal(2, receivedIds.Count);
        Assert.NotEqual(receivedIds[0], receivedIds[1]);
        Assert.All(receivedIds, id => Assert.NotEqual(Guid.Empty, id));
    }
}
