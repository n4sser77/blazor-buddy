using System;

namespace BlazorBuddy.Models
{
    public enum NotificationType
    {
        ChatMessage,
        ChatGroupInvite,
        FriendRequest
    }

    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Title { get; set; }
        public required string Description { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
