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
        var title = "Test Notification";
        var description = "Test Description";
        var type = NotificationType.ChatMessage;
        Notification? receivedNotification = null;

        _sut.OnNotificationReceived += (notification) =>
        {
            receivedNotification = notification;
        };

        // Act
        _sut.ShowNotification(title, description, type);

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(title, receivedNotification.Title);
        Assert.Equal(description, receivedNotification.Description);
        Assert.Equal(type, receivedNotification.Type);
        Assert.NotEqual(Guid.Empty, receivedNotification.Id);
    }

    [Fact]
    public void ShowNotification_WithChatGroupInvite_CreatesCorrectNotificationType()
    {
        // Arrange
        var title = "Group Invite";
        var description = "You've been invited to join a group";
        var type = NotificationType.ChatGroupInvite;
        Notification? receivedNotification = null;

        _sut.OnNotificationReceived += (notification) =>
        {
            receivedNotification = notification;
        };

        // Act
        _sut.ShowNotification(title, description, type);

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(NotificationType.ChatGroupInvite, receivedNotification.Type);
    }

    [Fact]
    public void ShowNotification_WithFriendRequest_CreatesCorrectNotificationType()
    {
        // Arrange
        var title = "Friend Request";
        var description = "Someone sent you a friend request";
        var type = NotificationType.FriendRequest;
        Notification? receivedNotification = null;

        _sut.OnNotificationReceived += (notification) =>
        {
            receivedNotification = notification;
        };

        // Act
        _sut.ShowNotification(title, description, type);

        // Assert
        Assert.NotNull(receivedNotification);
        Assert.Equal(NotificationType.FriendRequest, receivedNotification.Type);
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
}
