using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BlazorBuddy.Models
{
    public record class ChatGroup
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<UserProfile> Users { get; set; } = [];
        public List<ChatMessage> Messages { get; set; } = [];

        public ChatGroup AddUser(UserProfile newUser)
        {
            if (Users.Any(u => u.Id == newUser.Id))
                throw new InvalidOperationException("User already in group.");

            return this with
            {
                Users = [.. Users, newUser]
            };

        }

        public ChatGroup AddMessage(UserProfile user, string message)
        {
            var msg = new ChatMessage
            {
                Content = message,
                FromUser = user,
            };

            return this with
            {
                Messages = [.. Messages, msg]
            };
        }
        public bool IsUserInGroup(string userId) => Users.Any(u => u.Id == userId);


    }
}

