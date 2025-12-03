using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services.Interfaces;

public interface IChatService
{
    // logic for interacting with the chat
    // creating messages and adding users, useage of the repo to update the chatgroup,
    // raise events when new messages accours and notify usrs in that chat group
    //event Action<ChatGroup> OnChatGroupUpdate;
    Task AddUserToChatGroup(Guid groupId, string userId);
    Task SendMessage(Guid groupId, string userId, string message);
    Task<ChatGroup> UpdateChatGroup(ChatGroup updatedChatgroup);
    Task<ChatGroup> CreateChatGroup(string title, List<UserProfile> members);
}
