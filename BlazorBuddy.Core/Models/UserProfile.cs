using BlazorBuddy.Core.Models;
using System;

namespace BlazorBuddy.Models
{
    public class UserProfile
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string ProfilePicture { get; set; } = "";

        public List<ChatGroup> ChatGroups { get; set; } = [];

        public List<FriendList> Friends { get; set; } = [];

        public UserProfile()
        {

        }

        public UserProfile(string id)
        {
            Id = id;
        }


    }
}
