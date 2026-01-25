using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services.Interfaces
{
    public interface INotificationService
    {
        event Action<Notification>? OnNotificationReceived;

        void ShowNotification(string title, string description, NotificationType type);
    }
}
