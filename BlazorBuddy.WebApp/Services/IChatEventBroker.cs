using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services
{
    public interface IChatEventBroker
    {
        event Action<ChatGroup>? OnChatGroupUpdate;

        void NotifyStateChanged(ChatGroup group);
    }
}