using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services;

public class ChatEventBroker : IChatEventBroker
{
    public event Action<ChatGroup>? OnChatGroupUpdate;

    public void NotifyStateChanged(ChatGroup group)
    {

        OnChatGroupUpdate?.Invoke(group);
    }

}