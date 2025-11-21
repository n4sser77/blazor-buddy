using System;
using System.Collections.Generic;

namespace BlazorBuddy.Models
{
    public class ChatGroup
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
<<<<<<< HEAD
        public List<UserProfile> ListUser { get; set; } = [];
        public List<ChatMessage> ListMessage { get; set; } = [];
=======
        public List<AppUser> Users { get; set; } = [];
        public List<ChatMessage> Messages { get; set; } = [];
>>>>>>> main

        public ChatGroup()
        {
            Id = Guid.NewGuid();
        }
    }
}
