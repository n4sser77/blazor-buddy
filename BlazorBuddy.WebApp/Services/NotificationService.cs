using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services;

public class NotificationService : INotificationService
{
    public event Action<Notification>? OnNotificationReceived;

    public void ShowNotification(string title, string description, NotificationType type)
    {
        var notification = new Notification
        {
            Title = title,
            Description = description,
            Type = type
        };

        OnNotificationReceived?.Invoke(notification);
    }
}
