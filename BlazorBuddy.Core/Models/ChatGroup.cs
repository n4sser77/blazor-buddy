using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BlazorBuddy.Models
{
    public class ChatGroup
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<UserProfile> Users { get; set; } = [];
        public List<ChatMessage> Messages { get; set; } = [];

        public void AddUser(UserProfile newUser)
        {
            if (IsUserInGroup(newUser.Id))
                throw new InvalidOperationException("User already in group.");

            Users.Add(newUser);
            newUser.ChatGroups.Add(this);

        }

        public void AddMessage(UserProfile user, string message)
        {
            var msg = new ChatMessage
            {
                Content = message,
                FromUser = user,
            };

            Messages.Add(msg);

        }
        public bool IsUserInGroup(string userId) => Users.Any(u => u.Id == userId);


    }
}

