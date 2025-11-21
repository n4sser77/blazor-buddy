using System;
using System.Collections.Generic;

namespace BlazorBuddy.Models
{
    public class ChatGroup
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<AppUser> ListUser { get; set; } = [];
        public List<ChatMessage> ListMessage { get; set; } = [];

        public ChatGroup()
        {
            Id = Guid.NewGuid();
        }
    }
}
