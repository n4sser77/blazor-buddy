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
        public List<StudyPage> StudyPages { get; set; } = [];
        public List<FriendShip> FriendShips { get; set; } = [];

        public UserProfile()
        {

        }

        public void LeaveChat(ChatGroup chatGroup)
        {
            if (!chatGroup.IsUserInGroup(this.Id))
                throw new ArgumentException("Cannot remove user from chat because user is not in chat group");
            var chat = ChatGroups.FirstOrDefault(c => c.Id == chatGroup.Id);
            if (chat == null)
                throw new ArgumentException($"The user doesn't seam to be a member of group {chatGroup.Id}");

            chat.Users.Remove(this);
            ChatGroups.Remove(chat);

        }


    }
}
