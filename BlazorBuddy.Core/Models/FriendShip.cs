using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Core.Models
{
    public class FriendShip
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = "";
        public string FriendId { get; set; } = "";
        public FriendStatus Status { get; set; } = FriendStatus.Pending;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

    }
    public enum FriendStatus
    {
        Pending,
        Accepted,
        Blocked
    }
}
