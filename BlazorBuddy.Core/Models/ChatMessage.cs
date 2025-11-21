using System;

namespace BlazorBuddy.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = "";
        public string FromUserId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        public ChatMessage(string fromUserId)
        {
            Id = Guid.NewGuid();
            FromUserId = fromUserId;
        }
    }
}
