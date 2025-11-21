using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorBuddy.Models
{
    public class ChatGroup
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<UserProfile> Users { get; set; } = [];
        public List<ChatMessage> Messages { get; set; } = [];


    }
}
