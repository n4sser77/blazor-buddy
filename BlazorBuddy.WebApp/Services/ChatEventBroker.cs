using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services;

public class ChatEventBroker : IChatEventBroker
{
    public event Action<ChatGroup>? OnChatGroupUpdate;

    public void NotifyStateChanged(ChatGroup group)
    {

        OnChatGroupUpdate?.Invoke(group);
    }

}