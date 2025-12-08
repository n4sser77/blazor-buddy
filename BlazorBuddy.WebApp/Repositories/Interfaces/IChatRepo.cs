using BlazorBuddy.Models;

namespace BlazorBuddy.Core.Interfaces
{
    public interface IChatRepo
    {
        public Task<ChatGroup?> GetChatGroupById(Guid id);
        public Task<List<ChatGroup>> GetChatGroupsByUserId(string userId);
        public Task<ChatGroup> CreateChatGroup(string title, List<UserProfile> users);

        public Task<ChatGroup?> UpdateChatGroup(ChatGroup newChatGroup);


    }
}