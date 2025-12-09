using BlazorBuddy.Models;

namespace BlazorBuddy.Core.Interfaces
{
    public interface IChatRepo
    {
        Task<ChatGroup?> GetChatGroupById(Guid id);
        Task<List<ChatGroup>> GetChatGroupsByUserId(string userId);
        Task<ChatGroup> CreateChatGroup(string title, List<UserProfile> users);

        Task<ChatGroup?> UpdateChatGroup(ChatGroup newChatGroup);
        Task<ChatGroup?> AddMessage(Guid chatId, string msg, UserProfile user);


    }
}