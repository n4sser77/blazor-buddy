using BlazorBuddy.Models;

namespace BlazorBuddy.Core.Interfaces
{
    public interface IChatRepo
    {
        public Task<ChatGroup> GetChatGroupById(Guid id);
        public Task<List<ChatGroup>> GetChatGroups();
        public Task<ChatGroup> CreateChatGroup();
        public Task<ChatGroup> UpdateChatGroup(ChatGroup newChatGroup);
        public Task<bool> DeleteChatGroup(ChatGroup chatGroup);

    }
}
